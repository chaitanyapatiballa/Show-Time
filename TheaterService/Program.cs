using DBModels.Db;
using Microsoft.EntityFrameworkCore;
using TheaterService.Repositories;
using TheaterService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register DbContext using shared DBModels project
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register your services and repositories
builder.Services.AddScoped<TheaterRepository>();
builder.Services.AddScoped<TheaterServices>();
builder.Services.AddScoped<MovieTheaterRepository>();
builder.Services.AddScoped<MovieTheaterServices>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
