using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using backend.Models;

namespace backend.Services.OtherService
{
    public class EmailRatingService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EmailRatingService> _logger;

        public EmailRatingService(IServiceProvider serviceProvider, ILogger<EmailRatingService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("EmailRatingService is starting.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(24));
            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            _logger.LogInformation("EmailRatingService is working.");
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var _context = scope.ServiceProvider.GetRequiredService<FpttickethubContext>();
                    var emailService = EmailService.Instance;

                    var oneDayAgo = DateTime.Now.Date.AddDays(-1);

                    var completedEvents = await _context.Events
                        .Where(e => e.EndTime.HasValue && e.EndTime.Value.Date == oneDayAgo)
                        .ToListAsync();

                    _logger.LogInformation($"Found {completedEvents.Count} events completed one day ago.");

                    foreach (var evt in completedEvents)
                    {
                        var ordersWithCheckedInTickets = await _context.Orders
                            .Include(o => o.Account)
                            .Include(o => o.Orderdetails)
                                .ThenInclude(od => od.TicketType)
                                    .ThenInclude(tt => tt.Event)
                            .Include(o => o.Orderdetails)
                                .ThenInclude(od => od.Tickets)
                            .Where(o => o.Orderdetails.Any(od => od.TicketType.EventId == evt.EventId
                                && od.Tickets.Any(t => t.IsCheckedIn == true)))
                            .ToListAsync();

                        _logger.LogInformation($"Found {ordersWithCheckedInTickets.Count} orders with checked-in tickets for event {evt.EventName}.");

                        foreach (var order in ordersWithCheckedInTickets)
                        {
                            var user = order.Account;
                            if (user != null)
                            {
                                var existingRating = await _context.Eventratings
                                    .FirstOrDefaultAsync(er => er.AccountId == user.AccountId && er.EventId == evt.EventId);

                                if (existingRating == null)
                                {
                                    var newRating = new Eventrating
                                    {
                                        EventId = evt.EventId,
                                        AccountId = user.AccountId,
                                        RatingDate = DateTime.UtcNow,
                                        Status = "Pending"
                                    };

                                    _context.Eventratings.Add(newRating);
                                    await _context.SaveChangesAsync();

                                    bool result = await emailService.SendRatingRequestMail(user.Email, user.FullName, evt.EventName, newRating.EventRatingId);
                                    if (result)
                                    {
                                        _logger.LogInformation($"Successfully sent rating request email to {user.Email} for event {evt.EventName}.");
                                    }
                                    else
                                    {
                                        _logger.LogWarning($"Failed to send rating request email to {user.Email} for event {evt.EventName}.");
                                        _context.Eventratings.Remove(newRating);
                                    }
                                }
                                else
                                {
                                    _logger.LogInformation($"Rating request already sent to {user.Email} for event {evt.EventName}.");
                                }
                            }
                            else
                            {
                                _logger.LogWarning($"User not found for order {order.OrderId}.");
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while sending rating request emails.");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("EmailRatingService is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}