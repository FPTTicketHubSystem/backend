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
    public class StaffRemoveService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<StaffRemoveService> _logger;

        public StaffRemoveService(IServiceProvider serviceProvider, ILogger<StaffRemoveService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StaffRemoveService is starting.");
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(24));
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _logger.LogInformation("StaffRemoveService is working.");
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var _context = scope.ServiceProvider.GetRequiredService<FpttickethubContext>();

                    var yesterday = DateTime.Now.Date.AddDays(-1);

                    var pastEvents = _context.Events
                        .Where(e => e.EndTime.HasValue && e.EndTime.Value.Date == yesterday && e.Status == "Đã duyệt")
                        .ToList();

                    _logger.LogInformation($"Found {pastEvents.Count} past events.");

                    foreach (var evt in pastEvents)
                    {
                        var staffs = _context.Eventstaffs
                            .Where(es => es.EventId == evt.EventId)
                            .ToList();

                        _logger.LogInformation($"Found {staffs.Count} staff for event {evt.EventName}.");

                        foreach (var staff in staffs)
                        {
                            _context.Eventstaffs.Remove(staff);
                            _context.SaveChanges();

                            var user = _context.Accounts.Find(staff.AccountId);
                            if (user != null && user.RoleId == 4)
                            {
                                var remainingStaffRoles = _context.Eventstaffs
                                    .Where(es => es.AccountId == staff.AccountId)
                                    .ToList();

                                if (!remainingStaffRoles.Any())
                                {
                                    user.RoleId = 2;
                                }
                            }
                        }

                        _context.SaveChanges();
                        _logger.LogInformation($"Removed all staff from event {evt.EventName}.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while removing staff from past events.");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("StaffRemoveService is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}