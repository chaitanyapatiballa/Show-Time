using Booking_Service.Repositories;
using Booking_Service.Services;
using DBModels.Db;
using Microsoft.EntityFrameworkCore;
using MovieService.Repositories;
using MovieService.Services;
using PaymentService.Repositories;
using PaymentService.Services;
using TheaterService.Repositories;
using TheaterService.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Register HttpClient factory for DI
builder.Services.AddHttpClient();

// ✅ Register Repositories & Services
builder.Services.AddScoped<BookingRepository>();
builder.Services.AddScoped<MovieRepository>();
builder.Services.AddScoped<TheaterRepository>();
builder.Services.AddScoped<PaymentRepository>();

builder.Services.AddScoped<BookingServices>();
builder.Services.AddScoped<MovieServices>();
builder.Services.AddScoped<TheaterServices>();
builder.Services.AddScoped<PaymentServices>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
