using BusinessLogic;
using DataAccessLayer.Repositories;
using DBModels.Models;
using MessagingLibrary;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PaymentService.Logic;
using PaymentService.Repositories;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// --- JSON & Controller Setup ---
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// --- EF Core ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// RabbitMQ
builder.Services.AddSingleton<IRabbitMQPublisher>(sp =>
    new RabbitMQPublisher(configuration["RabbitMQ:ConnectionString"]));
builder.Services.AddSingleton<IRabbitMQConsumer>(sp =>
    new RabbitMQConsumer(configuration["RabbitMQ:ConnectionString"]));

// --- Register Repositories & Logic ---
builder.Services.AddScoped<PaymentRepository>();
builder.Services.AddScoped<BillingsummaryRepository>();
builder.Services.AddScoped<BookingRepository>();
builder.Services.AddScoped<PaymentLogic>();

// --- JWT Authentication (Raw token - no "Bearer ") ---
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var config = builder.Configuration;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config["Jwt:Issuer"],
            ValidAudience = config["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!))
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault();
                if (!string.IsNullOrWhiteSpace(token))
                {
                    context.Token = token; // ⬅️ Pick raw token
                }
                return Task.CompletedTask;
            }
        };
    });

// --- Swagger with Raw Token Support ---
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PaymentService API", Version = "v1" });

    c.AddSecurityDefinition("Authorization", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Description = "Enter JWT token only (no 'Bearer' prefix required)"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Authorization",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// --- Middleware Pipeline ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();    // Must be before UseAuthorization
app.UseAuthorization();

app.MapControllers();
app.Run();
