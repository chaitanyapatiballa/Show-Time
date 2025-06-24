using BusinessLogic;
using DataAccessLayer.Repositories;
using DBModels.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Clear default sources
builder.Configuration.Sources.Clear();

// Get environment
var env = builder.Environment;

// Find and load envsettings.json from the solution root
var configPath = FindSolutionRootContaining("envsettings.json");
builder.Configuration.AddJsonFile(configPath, optional: false, reloadOnChange: true);

// Optionally add user secrets
if (env.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

// Local function to walk up the directory tree
string FindSolutionRootContaining(string fileName)
{
    var dir = new DirectoryInfo(Directory.GetCurrentDirectory());

    while (dir != null)
    {
        var fullPath = Path.Combine(dir.FullName, fileName);
        if (File.Exists(fullPath))
            return fullPath;

        dir = dir.Parent;
    }

    throw new FileNotFoundException($"{fileName} not found in any parent directory.");
}




builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure PostgreSQL connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register custom services and repositories
builder.Services.AddScoped<MovieRepository>();
builder.Services.AddScoped<MovieLogic>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
