using MessagingLibrary;
using NotificationService;

var builder = Host.CreateApplicationBuilder(args);

// Configuration
var configuration = builder.Configuration;

// RabbitMQ
builder.Services.AddSingleton<IRabbitMQConsumer>(sp =>
    new RabbitMQConsumer(configuration["RabbitMQ:ConnectionString"]));

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
