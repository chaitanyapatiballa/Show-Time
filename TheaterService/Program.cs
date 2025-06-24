using BusinessLogic;
using DataAccessLayer.Repositories;
using DBModels.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);


builder.Configuration.Sources.Clear();


var env = builder.Environment;


var configPath = FindSolutionRootContaining("envsettings.json");
builder.Configuration.AddJsonFile(configPath, optional: false, reloadOnChange: true);

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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection
builder.Services.AddScoped<TheaterRepository>();
builder.Services.AddScoped<TheaterLogic>();

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
