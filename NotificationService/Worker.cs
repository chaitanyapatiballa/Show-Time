using MessagingLibrary;

namespace NotificationService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IRabbitMQConsumer _consumer;

    public Worker(ILogger<Worker> logger, IRabbitMQConsumer consumer)
    {
        _logger = logger;
        _consumer = consumer;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Notification Service is starting.");

        // Consume Booking Events
        _consumer.StartConsuming("booking-queue", (message) =>
        {
            _logger.LogInformation($"[Notification] Sending Booking Confirmation: {message}");
            // Simulate Email/SMS sending delay
            Task.Delay(100).Wait();
        });

        // Consume Payment Events
        _consumer.StartConsuming("payment-queue", (message) =>
        {
            _logger.LogInformation($"[Notification] Sending Payment Receipt: {message}");
            // Simulate Email/SMS sending delay
            Task.Delay(100).Wait();
        });

        return Task.CompletedTask;
    }
}
