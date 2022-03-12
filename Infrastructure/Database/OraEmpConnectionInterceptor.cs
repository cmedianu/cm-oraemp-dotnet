using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using OraEmp.Application.Services;

namespace OraEmp.Infrastructure.Persistence;

public class OraEmpConnectionInterceptor :DbConnectionInterceptor
{
    private readonly ILogger _logger;
    private AppStateInfo _identity;

    public OraEmpConnectionInterceptor(ILoggerFactory loggerFactory, AppStateInfo identity)
    {
        _logger = loggerFactory.CreateLogger(GetType().FullName!);
        _identity = identity;
    }

    public override Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData,
        CancellationToken cancellationToken = new CancellationToken())
    {
        _logger.LogInformation("Opened Connection");

        OracleConnection conn = connection as OracleConnection;
        conn.ClientId = _identity.State.Username + "@" + _identity.State.RemoteIpAddress;
        return base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
    }

    public override ValueTask<InterceptionResult> ConnectionClosingAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
    {
        _logger.LogInformation("Closing Connection");
        OracleConnection conn = connection as OracleConnection;
        conn.ClientId = "CLOSED";
        return base.ConnectionClosingAsync(connection, eventData, result);
    }
}