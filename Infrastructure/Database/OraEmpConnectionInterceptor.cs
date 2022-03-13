using System.Data;
using System.Data.Common;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using OraEmp.Application.Common;
using OraEmp.Application.Services;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace OraEmp.Infrastructure.Persistence;

public class OraEmpConnectionInterceptor :DbConnectionInterceptor
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly IAppStateInfoService _stateInfoService;
    private IDbSessionManagement _dbSessionManagement;

    public OraEmpConnectionInterceptor(ILoggerFactory loggerFactory,AuthenticationStateProvider authenticationStateProvider,IAppStateInfoService stateInfoService, IDbSessionManagement dbSessionManagement)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _stateInfoService = stateInfoService;
        _dbSessionManagement = dbSessionManagement;
        //_logger = loggerFactory.CreateLogger(GetType().FullName!);
    }

    public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData,
        CancellationToken cancellationToken = new CancellationToken())
    {
        Log.Information("Opened Connection");
        OracleConnection conn = connection as OracleConnection;
        AuthenticationState authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var sessionId = _stateInfoService.State.SessionId;
        var userId = authenticationState.User.Identity.Name;
        if (sessionId is null)
        {
            sessionId =  _dbSessionManagement.GetNewSessionId(userId);
            Log.Information("Creating new session {sessionId} for {userId}",sessionId,userId);
            _stateInfoService.State.SessionId = sessionId;
        }
        else
        {
            Log.Information("Reusing session {sessionId} for {userId}",sessionId,userId);
        }

        var newClientId = authenticationState.User.Identity.Name + "/" + sessionId +"@" + _stateInfoService.State.RemoteIpAddress;
        Log.Debug("ConnectionOpenedAsync: {ClientId}",newClientId);
        conn.ClientId = newClientId;
    }

    public override ValueTask<InterceptionResult> ConnectionClosingAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
    {
        Log.Information("Closing Connection");
        OracleConnection conn = connection as OracleConnection;
        conn.ClientId = "CLOSED";
        return base.ConnectionClosingAsync(connection, eventData, result);
    }
}