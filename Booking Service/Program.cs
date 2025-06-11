
using BookingService.Repositories;
using BookingService.Services;
using DBModels.Db;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services and repositories
builder.Services.AddScoped<BookingServices>();
builder.Services.AddScoped<BookingRepository>();

// Add HttpClients
builder.Services.AddHttpClient("MovieService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7105");
});
builder.Services.AddHttpClient("TheaterService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7106");
});
builder.Services.AddHttpClient("PaymentService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7107");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();

app.Run();

