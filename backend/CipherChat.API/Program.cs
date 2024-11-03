using CipherChat.API;
using CipherChat.API.Hubs;
using CipherChat.Ciphers;
using CipherChat.Ciphers.CaesarCipher;
using CipherChat.Ciphers.PlayfairCipher;
using CipherChat.Ciphers.PolibiusCipher;
using CipherChat.Ciphers.VigenereCipher;
using CipherChat.Domain.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/myapp.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddStackExchangeRedisCache(options =>
    options.Configuration = builder.Configuration.GetConnectionString("Redis"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddSignalR();
builder.Services.AddScoped<CaesarCipherService>();
builder.Services.AddScoped<VigenereCipherService>();
builder.Services.AddScoped<PlayfairCipherService>();
builder.Services.AddScoped<PolibiusCipherService>();
        
builder.Services.AddScoped<ICipherFactory, CipherFactory>();

var chatExtension = new ChatExtension();
chatExtension.ConfigureServices(builder.Services);

var app = builder.Build();

app.UseCors("AllowFrontend");

app.MapHub<ChatHub>("/chat");

try
{
    Log.Information("Starting up the application...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed.");
}
finally
{
    Log.CloseAndFlush();
}