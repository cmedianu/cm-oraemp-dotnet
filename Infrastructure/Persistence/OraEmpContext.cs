using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace OraEmp.Infrastructure.Persistence;

public class OraEmpContext : OraEmpContextBase
{
    private readonly ILoggerFactory _loggerFactory;
    private readonly OraEmpConnectionInterceptor _interceptor;

    public OraEmpContext(DbContextOptions<OraEmpContext> options, ILoggerFactory loggerFactory, OraEmpConnectionInterceptor interceptor) : base(options)
    {
        _loggerFactory = loggerFactory;
        _interceptor = interceptor;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseLoggerFactory(_loggerFactory);
        optionsBuilder.AddInterceptors(_interceptor);
    }
}