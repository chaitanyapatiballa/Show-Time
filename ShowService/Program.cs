

var builder = WebApplication.CreateBuilder(args);

// DB

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
