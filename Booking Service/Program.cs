using Booking_Service.HttpClients;
using Booking_Service.Repositories;
using Booking_Service.Services;
using DBModels.Db;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Typed HttpClients
builder.Services.AddHttpClient<IMovieServiceClient, MovieServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5100/"); // MovieService
});

builder.Services.AddHttpClient<ITheaterServiceClient, TheaterServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5101/"); // TheaterService
});

builder.Services.AddHttpClient<IPaymentServiceClient, PaymentServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5102/"); // PaymentService
});

// Register Repositories and Services
builder.Services.AddScoped<BookingRepository>();
builder.Services.AddScoped<BookingServices>();

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
