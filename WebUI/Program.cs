using System.Reflection;
using System.Runtime.InteropServices;
using AutoMapper;
using BlazorTable;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using OraEmp.Application.Dto;
using OraEmp.Application.Services;
using OraEmp.Infrastructure.Persistence;
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
// Add services to the container, infrastructure
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor(c => c.DetailedErrors = true);

// Useful Table library: https://github.com/IvanJosipovic/BlazorTable
builder.Services.AddBlazorTable();

// Add this application's services to DI
builder.Services.AddOraEmpServices();

/*
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\\TEMP\\dataprotection\\"));
*/

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

builder.Services.AddScoped<IDbSessionManagement>(s =>
    new DbSessionManagement(connectionString, s.GetRequiredService<ILogger>()));
builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
builder.Services.AddScoped<IAppStateInfoService, AppStateInfoService>();
builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("CoolCitiesOnly", policy => policy.RequireClaim("city", "Brasov"));
        options.AddPolicy("ManagerOnly", policy => policy.RequireRole("SUPERMANAGER", "MANAGER"));
    }
);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

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

// Set up validation
//builder.Services.AddTransient<IValidator<DepartmentForm>, DepartmentValidator>();
// This is the assembly containing the validation.
// As an alternative, remove DisableAssemblyScanning="@true" from the form, or add using DI
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.Load("Application")));

// Set up bean mapping
MapperConfiguration mapperConfig = new(cfg => { cfg.AddProfile<WebFormToDomainMappingProfile>(); });
var mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddSingleton(mapperConfig);

// Captures initial user info, holds DB session ID
builder.Services.AddScoped<AppStateInfoService>();

/// END SERVICES

var app = builder.Build();
// app.UsePathBase("/empdept");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSerilogRequestLogging(opts
    => opts.EnrichDiagnosticContext = (diagnosticsContext, httpContext) =>
    {
        var request = httpContext.Request;
        diagnosticsContext.Set("User-Agent", request.Headers["User-Agent"]);
    });


app.UseAuthentication();
app.UseAuthorization();


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();