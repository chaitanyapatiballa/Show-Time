using BusinessLogic;
using DataAccessLayer.Repositories;
using DBModels.Models;
using Microsoft.EntityFrameworkCore;
using PaymentService.Logic;
using PaymentService.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection
builder.Services.AddScoped<PaymentRepository>();
builder.Services.AddScoped<BillingsummaryRepository>();
builder.Services.AddScoped<BookingRepository>(); 
builder.Services.AddScoped<PaymentLogic>();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
