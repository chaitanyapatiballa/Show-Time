using BusinessLogic;
using DataAccessLayer.Repositories;
using DBModels.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DB
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// DI
builder.Services.AddScoped<ShowRepository>();
builder.Services.AddScoped<ShowManager>();

builder.Services.AddHttpClient();

// API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Use HTTPS redirection if needed
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
