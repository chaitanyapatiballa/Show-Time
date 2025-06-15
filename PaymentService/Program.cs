using BusinessLogic;
using DataAccessLayer.Repositories;
using DBModels.Models;
using Microsoft.EntityFrameworkCore;
using PaymentService.Repositories;
using PaymentService.Services;

var builder = WebApplication.CreateBuilder(args);

//  Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//  Register repositories
builder.Services.AddScoped<PaymentRepository>();
builder.Services.AddScoped<BillingSummaryRepository>(); 

//  Register services
builder.Services.AddScoped<BillingSummaryService>();
builder.Services.AddScoped<PaymentService.Services.PaymentService>();

builder.Services.AddHttpClient();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();
