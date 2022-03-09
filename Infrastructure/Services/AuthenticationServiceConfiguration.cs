using Hides.Infrastructure.Persistence.Util;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using OraEmp.Infrastructure.Identity;

namespace OraEmp.Infrastructure.Services
{
    public static class AuthenticationServiceConfiguration
    {
        public static IServiceCollection AddEmpAuthenticationServices(this IServiceCollection services,
            Action<ConfigOptions> configureOptions)
        {
            services.Configure<ConfigOptions>(configureOptions);
            services.AddScoped<AuthenticationStateProvider, EmpAuthenticationStateProvider>();
            return services;
        }
    }
}