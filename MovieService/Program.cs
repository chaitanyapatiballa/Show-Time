using Microsoft.EntityFrameworkCore;
using MovieService.Data;
using MovieService.Repositories;
using MovieService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext with PostgreSQL connection
builder.Services.AddDbContext<MovieDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add repository and service DI
builder.Services.AddScoped<MovieRepository>();
builder.Services.AddScoped<MovieServices>();

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
