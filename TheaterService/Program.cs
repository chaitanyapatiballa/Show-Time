using BusinessLogic;
using DataAccessLayer.Repositories;
using DBModels.Models;
using MessagingLibrary;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Services
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// RabbitMQ
builder.Services.AddSingleton<IRabbitMQPublisher>(sp =>
    new RabbitMQPublisher(configuration["RabbitMQ:ConnectionString"]));
builder.Services.AddSingleton<IRabbitMQConsumer>(sp =>
    new RabbitMQConsumer(configuration["RabbitMQ:ConnectionString"]));

// Dependency Injection
builder.Services.AddScoped<TheaterRepository>();
builder.Services.AddScoped<TheaterLogic>();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
