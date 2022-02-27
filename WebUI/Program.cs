using System.Runtime.InteropServices;
using AutoMapper;
using Blazored.SessionStorage;
using BlazorTable;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using OraEmp.Application.Dto;
using OraEmp.Application.Services;
using OraEmp.Infrastructure.Persistence;
using OraEmp.Infrastructure.Services;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    var logfile = isWindows ? @"c:\TEMP\OraEmp-.txt" : "../OraEmp-.txt";

    var cfg = loggerConfiguration.Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithEnvironmentName()
        .Enrich.FromLogContext()
        .ReadFrom.Configuration(context.Configuration) // This is so it picks up the settings from appsettings.*.json
        .WriteTo.Console(
            outputTemplate:
            "[{Timestamp:HH:mm:ss} ({SourceContext}) {Level:u3}] {Message:lj}{NewLine}{Exception}");

    cfg.WriteTo.File(logfile,
        outputTemplate: "[{Timestamp:HH:mm:ss} ({SourceContext}) {Level:u3}] {Message:lj}{NewLine}{Exception}"
        , fileSizeLimitBytes: 10000000, restrictedToMinimumLevel: LogEventLevel.Information, retainedFileCountLimit: 4,
        rollingInterval: RollingInterval.Day);
});
// Add services to the container, infrastructure
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddBlazorTable();

// Add services to the container, Application
builder.Services.AddOraEmpServices();
builder.Services.AddBlazoredSessionStorage(config =>
    config.JsonSerializerOptions.WriteIndented = true
);

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\\TEMP\\dataprotection\\"));

// set up connection name
var defaultConnectionName = builder.Configuration.GetConnectionString("Default");
var connectionString = builder.Configuration["ConnectionStrings:" + defaultConnectionName];

if (string.IsNullOrEmpty(connectionString))
    throw new Exception(
        $"The \"Default\" Datasource could not be found in your secrets file. Look in appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json and in %APPDATA%\\Microsoft\\UserSecrets\\");

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
        options.EnableSensitiveDataLogging();
    });

MapperConfiguration mapperConfig = new(cfg => {
    cfg.AddProfile<WebFormToDomainMappingProfile>();
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddSingleton(mapperConfig);


var app = builder.Build();

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
        diagnosticsContext.Set("Custom Header value", request.Headers["custom-header-value"]);
    });

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();