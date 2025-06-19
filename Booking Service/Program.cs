using BusinessLogic;
using DataAccessLayer.Repositories;
using DBModels.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PaymentService.Repositories;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ✅ Load shared config from DBModels (appsettings.shared.json)
var sharedConfigPath = Path.Combine(AppContext.BaseDirectory, "appsettings.shared.json");
builder.Configuration.AddJsonFile(sharedConfigPath, optional: false, reloadOnChange: true);

// Add Controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "BookingService", Version = "v1" });

    c.AddSecurityDefinition("JWT", new()
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "JWT",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter JWT token (no Bearer prefix required)"
    });

    c.AddSecurityRequirement(new()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "JWT"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ✅ Configure PostgreSQL DbContext using shared config
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services
builder.Services.AddScoped<BookingRepository>();
builder.Services.AddScoped<BillingsummaryRepository>();
builder.Services.AddScoped<PaymentRepository>();
builder.Services.AddScoped<BookingLogic>();

// Configure HTTP clients
builder.Services.AddHttpClient();
builder.Services.AddHttpClient("MovieService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7105");
});
builder.Services.AddHttpClient("TheaterService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7106");
});
builder.Services.AddHttpClient("PaymentService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7107");
});
builder.Services.AddHttpClient("AuthService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7255");
});

// Configure JWT Authentication
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrEmpty(token))
                {
                    context.Token = token;
                }
                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

// Enable Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
