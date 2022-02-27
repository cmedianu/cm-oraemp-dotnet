using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace OraEmp.Infrastructure.Persistence;

public class OraEmpConnectionInterceptor :DbConnectionInterceptor
{
    private readonly ILogger _logger;

    public OraEmpConnectionInterceptor(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger(GetType().FullName!);
    }

    public override Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData,
        CancellationToken cancellationToken = new CancellationToken())
    {
        _logger.LogInformation("Opened Connection");
        return base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
    }

    public override ValueTask<InterceptionResult> ConnectionClosingAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
    {
        _logger.LogInformation("Closing Connection");
        return base.ConnectionClosingAsync(connection, eventData, result);
    }
}