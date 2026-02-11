using System;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Booking_Service.Workers
{
    public class ShowInstanceWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ShowInstanceWorker> _logger;

        public ShowInstanceWorker(IServiceProvider serviceProvider, ILogger<ShowInstanceWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("ShowInstanceWorker is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("ShowInstanceWorker running at: {time}", DateTimeOffset.Now);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var bookingLogic = scope.ServiceProvider.GetRequiredService<BookingLogic>();
                        await bookingLogic.GenerateNextDayShowinstancesAsync();
                    }

                    _logger.LogInformation("Next day show instances generated successfully.");
                    
                    // Run once every 24 hours (or at a specific time)
                    // For now, let's wait 24 hours
                    await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while generating show instances.");
                    // Apply a backoff strategy if needed, here just waiting a bit before retry
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }
        }
    }
}
