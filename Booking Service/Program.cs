using Booking_Service.Repositories;
using Booking_Service.Services;
using DBModels.Db;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>();

builder.Services.AddScoped<BookingRepository>();
builder.Services.AddScoped<BookingServices>();

// Named HTTP Clients for each external service
builder.Services.AddHttpClient("MovieService", client =>
{
    client.BaseAddress = new Uri("http://localhost:7105");
});
builder.Services.AddHttpClient("TheaterService", client =>
{
    client.BaseAddress = new Uri("http://localhost:7106");
});
builder.Services.AddHttpClient("PaymentService", client =>
{
    client.BaseAddress = new Uri("http://localhost:7107");
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();

