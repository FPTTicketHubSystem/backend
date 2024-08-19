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
    public class EmailReminderService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EmailReminderService> _logger;

        public EmailReminderService(IServiceProvider serviceProvider, ILogger<EmailReminderService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("EmailReminderService is starting.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(24));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("EmailReminderService is working.");
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var _context = scope.ServiceProvider.GetRequiredService<FpttickethubContext>();
                    var emailService = EmailService.Instance;

                    var tomorrow = DateTime.UtcNow.Date.AddDays(1);

                    var upcomingEvents = _context.Events
                        .Where(e => EF.Functions.DateDiffDay(e.StartTime, tomorrow) == 0)
                        .ToList();

                    _logger.LogInformation($"Found {upcomingEvents.Count} upcoming events.");

                    foreach (var evt in upcomingEvents)
                    {
                        var tickets = _context.Tickets
                            .Include(t => t.OrderDetail)
                            .ThenInclude(od => od.Order)
                            .Where(t => t.OrderDetail.TicketType.Event.EventId == evt.EventId && t.OrderDetail.Order.Status == "Đã thanh toán")
                            .ToList();

                        _logger.LogInformation($"Found {tickets.Count} tickets for event {evt.EventName}.");

                        foreach (var ticket in tickets)
                        {
                            var orderDetail = ticket.OrderDetail;
                            if (orderDetail == null)
                            {
                                _logger.LogWarning($"OrderDetail not found for ticket {ticket.TicketId}.");
                                continue;
                            }

                            var order = orderDetail.Order;
                            if (order == null)
                            {
                                _logger.LogWarning($"Order not found for OrderDetail {orderDetail.OrderDetailId}.");
                                continue;
                            }

                            var user = _context.Accounts.Find(order.AccountId);
                            if (user != null)
                            {
                                var result = emailService.SendEventReminderMail(user.Email, user.FullName, evt.EventName, evt.StartTime.Value, evt.Location, evt.Address).Result;
                                if (result)
                                {
                                    _logger.LogInformation($"Successfully sent email to {user.Email}.");
                                }
                                else
                                {
                                    _logger.LogWarning($"Failed to send email to {user.Email}.");
                                }
                            }
                            else
                            {
                                _logger.LogWarning($"User not found for account ID {order.AccountId}.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while sending event reminder emails.");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("EmailReminderService is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}