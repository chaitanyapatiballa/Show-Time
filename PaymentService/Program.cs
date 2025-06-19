using BusinessLogic;
using DataAccessLayer.Repositories;
using DBModels.Models;
using Microsoft.EntityFrameworkCore;
using PaymentService.Logic;
using PaymentService.Repositories;

var builder = WebApplication.CreateBuilder(args);

var sharedConfigPath = Path.Combine(AppContext.BaseDirectory, "appsettings.shared.json");
builder.Configuration.AddJsonFile(sharedConfigPath, optional: false, reloadOnChange: true);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// Dependency Injection
builder.Services.AddScoped<PaymentRepository>();
builder.Services.AddScoped<PaymentLogic>();
builder.Services.AddScoped<BillingsummaryRepository>();

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
