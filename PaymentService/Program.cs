using Microsoft.EntityFrameworkCore;
using PaymentService.Repositories;
using PaymentService.Services;
using DBModels.Db;

var builder = WebApplication.CreateBuilder(args);

// 🔗 Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔗 Register services
builder.Services.AddScoped<PaymentRepository>();
builder.Services.AddScoped<BillingSummaryService>();
builder.Services.AddScoped<PaymentService.Services.PaymentService>(); 

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();
