using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Oracle.ManagedDataAccess.Client;
using OraEmp.Application.Services;
using OraEmp.Infrastructure.Persistence;

namespace Infrastructure.UnitTests;

public class Startup
{
    private IConfiguration? Configuration { get; set; }

    public void ConfigureServices(IServiceCollection services)
    {
        var builder = new ConfigurationBuilder().AddUserSecrets<Startup>()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory)?.FullName)
            .AddJsonFile("appsettings.json", false);
        Configuration = builder.Build();
        var connectionStringName = Configuration.GetConnectionString("Default");
        var connectionString = Configuration.GetConnectionString(connectionStringName) ?? throw new ArgumentNullException("Configuration.GetConnectionString(connectionStringName)");
        //services.AddTransient<IDependency, DependencyClass>();
        services.AddSingleton<OraEmpConnectionInterceptor>();
        services.AddOraEmpServices();

        services.AddDbContextFactory<DataContext>(
            options =>
            {
                options.UseOracle(connectionString,
                    o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
                options.EnableSensitiveDataLogging();
            }, ServiceLifetime.Scoped);

        services.AddDbContext<DataContext>(options =>
        {
            options.UseOracle(connectionString,
                o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            options.EnableSensitiveDataLogging();
        });
        // OracleConfiguration.OracleDataSources.Add(connectionStringName, connectionString);

        services.AddScoped<IDbSessionManagement>(s => new DbSessionManagement(connectionString));
        OracleConfiguration.BindByName = true;

        // Set tracing options
        OracleConfiguration.TraceOption = 1;
        OracleConfiguration.TraceFileLocation = @"c:\TEMP";
        // Uncomment below to generate trace files
        //OracleConfiguration.TraceLevel = 7;

    }
}