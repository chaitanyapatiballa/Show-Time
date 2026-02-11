using BusinessLogic;
using BusinessLogic.Middleware;
using DataAccessLayer.Repositories;
using DBModels.Models;
using MessagingLibrary;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using BusinessLogic.Middleware;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure PostgreSQL connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// RabbitMQ
builder.Services.AddSingleton<IRabbitMQPublisher>(sp =>
    new RabbitMQPublisher(configuration["RabbitMQ:ConnectionString"]));
builder.Services.AddSingleton<IRabbitMQConsumer>(sp =>
    new RabbitMQConsumer(configuration["RabbitMQ:ConnectionString"]));

// Register custom services and repositories
builder.Services.AddScoped<MovieRepository>();
builder.Services.AddScoped<MovieRepository>();
builder.Services.AddScoped<MovieLogic>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<EventLogic>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
