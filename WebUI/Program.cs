using System.Reflection;
using System.Runtime.InteropServices;
using AutoMapper;
using BlazorTable;
using FluentValidation;
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
builder.Services.AddServerSideBlazor(c => c.DetailedErrors = true);

builder.Services.AddBlazorTable();

// Add services to the container, Application
builder.Services.AddOraEmpServices();

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\\TEMP\\dataprotection\\"));

// set up connectionString
var defaultConnectionName = builder.Configuration.GetConnectionString("Default");
var connectionString = builder.Configuration["ConnectionStrings:" + defaultConnectionName];
if (string.IsNullOrEmpty(connectionString))
    throw new Exception(
        $"The \"Default\" Datasource could not be found in your secrets file. Look in appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json and in %APPDATA%\\Microsoft\\UserSecrets\\");

builder.Services.AddScoped<IDbSessionManagement>(s => new DbSessionManagement(connectionString,s.GetRequiredService<ILogger>()));

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
        options.EnableSensitiveDataLogging();
    });

//builder.Services.AddTransient<IValidator<DepartmentForm>, DepartmentValidator>();

// This is the assembly containing the validation.
// As an alternative, remove DisableAssemblyScanning="@true" from the form, or add using DI
builder.Services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssembly(Assembly.Load("Application")));

MapperConfiguration mapperConfig = new(cfg => { cfg.AddProfile<WebFormToDomainMappingProfile>(); });
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddSingleton(mapperConfig);

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