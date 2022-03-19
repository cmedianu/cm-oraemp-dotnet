using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.EntityFrameworkCore;
using OraEmp.Application.Services;
using OraEmp.Infrastructure.Persistence;
using OraEmp.Infrastructure.Services;
using Serilog;
using Serilog.Events;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfiguration) =>
    {
        var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        var logfile = isWindows ? @"c:\TEMP\OraEmp-.txt" : "../OraEmp-.txt";

        var cfg = loggerConfiguration.Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentName()
            .Enrich.FromLogContext()
            .ReadFrom
            .Configuration(context.Configuration) // This is so it picks up the settings from appsettings.*.json
            .WriteTo.Console(
                outputTemplate:
                "[{Timestamp:HH:mm:ss} ({SourceContext}) {Level:u3}] {Message:lj}{NewLine}{Exception}");

        cfg.WriteTo.File(logfile,
            outputTemplate: "[{Timestamp:HH:mm:ss} ({SourceContext}) {Level:u3}] {Message:lj}{NewLine}{Exception}"
            , fileSizeLimitBytes: 10000000, restrictedToMinimumLevel: LogEventLevel.Information,
            retainedFileCountLimit: 4,
            rollingInterval: RollingInterval.Day);
    })
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        // alternate way of storing secrets, for Docker
        var path = Path.Combine(
            Directory.GetCurrentDirectory(), "secrets");
        config.AddKeyPerFile(path, true);
    });
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// find the right connection string.
var defaultConnectionName = builder.Configuration.GetConnectionString("Default");
string connectionString;
connectionString = builder.Configuration["ConnectionStrings:" + defaultConnectionName];
if (connectionString is not null)
{
    Log.Information("Connection string {defaultConnectionName} retrieved from secrets",defaultConnectionName);
}
else
{
    // Using docker
    connectionString = builder.Configuration[defaultConnectionName];
    if (connectionString is not null)
    {
        Log.Information("Connection string {defaultConnectionName} retrieved from key per file",defaultConnectionName);
    }
}

if (string.IsNullOrEmpty(connectionString))
    throw new Exception(
        $"The \"Default\" Datasource could not be found in your secrets file. Look in appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json and in %APPDATA%\\Microsoft\\UserSecrets\\");
// End Connection String

builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

builder.Services.AddScoped<IDbSessionManagement>(s =>
    new DbSessionManagement(connectionString, s.GetRequiredService<ILogger>()));

// Add services to the container, Application
builder.Services.AddOraEmpServices();
// Captures initial user info, holds DB session ID
builder.Services.AddScoped<IAppStateInfoService, AppStateInfoService>();


builder.Services.AddDbContextFactory<DataContext>(
    options =>
    {
        options.UseOracle(connectionString,
            o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        options.EnableSensitiveDataLogging();
    }, ServiceLifetime.Scoped);

builder.Services.AddDbContext<DataContext>(
    options =>
    {
        options.UseOracle(connectionString,
            o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        options.EnableSensitiveDataLogging(); // also log bind parameters
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.MapGet("/xx",)

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();