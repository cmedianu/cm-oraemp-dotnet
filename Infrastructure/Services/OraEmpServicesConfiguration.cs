using Application.Common;
using Microsoft.Extensions.DependencyInjection;
using OraEmp.Infrastructure.Persistence;
using OraEmp.Infrastructure.Services;

namespace OraEmp.Application.Services
{
    public static class OraEmpServicesConfiguration
    {
        public static void AddOraEmpServices(this IServiceCollection services)
        {
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<OraEmpConnectionInterceptor>();
            // services.AddScoped<ISessionService,SimpleMemorySessionImpl>();
            //services.AddScoped<ISessionService,TabStorageSessionImpl>();
        }
    }
}