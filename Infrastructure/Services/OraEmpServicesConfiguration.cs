using Application.Common;
using Microsoft.Extensions.DependencyInjection;
using OraEmp.Application.Services;
using OraEmp.Infrastructure.Persistence;
using OraEmp.Infrastructure.Session;

namespace OraEmp.Infrastructure.Services;

public static class OraEmpServicesConfiguration
{
    public static void AddOraEmpServices(this IServiceCollection services)
    {
        services.AddScoped<IHrService,HrService>();
        services.AddScoped<ISessionService,SimpleMemorySessionImpl>();
        services.AddSingleton<OraEmpConnectionInterceptor>();
    }
}