using System.Data;
using System.Data.Common;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Oracle.ManagedDataAccess.Client;
using OraEmp.Application.Services;
using Serilog;
using ILogger = Serilog.ILogger;


namespace OraEmp.Infrastructure.Persistence;

public class OraEmpConnectionInterceptor :DbConnectionInterceptor
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly IAppStateInfoService _stateInfoService;
    private IDbSessionManagement _dbSessionManagement;
    private readonly ILogger _logger;

    public OraEmpConnectionInterceptor(ILogger logger, AuthenticationStateProvider authenticationStateProvider,IAppStateInfoService stateInfoService, IDbSessionManagement dbSessionManagement)
    {
        _authenticationStateProvider = authenticationStateProvider;
        _stateInfoService = stateInfoService;
        _dbSessionManagement = dbSessionManagement;
        _logger = logger?.ForContext<OraEmpConnectionInterceptor>() ?? throw new ArgumentNullException(nameof(_logger));
    }

    public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData,
        CancellationToken cancellationToken = new CancellationToken())
    {
        _logger.Information("Opened Connection");
        OracleConnection conn = connection as OracleConnection;
        AuthenticationState authenticationState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        var sessionId = _stateInfoService.State.SessionId;
        var userId = authenticationState.User.Identity.Name;
        if (sessionId is null)
        {
            sessionId =  _dbSessionManagement.GetNewSessionId(userId);
            _logger.Information("Creating new session {sessionId} for user {userId}",sessionId,userId);
            _stateInfoService.State.SessionId = sessionId;
        }
        else
        {
            _logger.Information("Reusing session {sessionId} for user {userId}",sessionId,userId);
        }

        var newClientId = authenticationState.User.Identity.Name + "/" + sessionId +"@" + _stateInfoService.State.RemoteIpAddress;
        _logger.Debug("ConnectionOpenedAsync: {ClientId}",newClientId);
        conn.ClientId = newClientId;
    }

    public override ValueTask<InterceptionResult> ConnectionClosingAsync(DbConnection connection, ConnectionEventData eventData, InterceptionResult result)
    {
        _logger.Information("Closing Connection");
        OracleConnection conn = connection as OracleConnection;
        conn.ClientId = "CLOSED";
        return base.ConnectionClosingAsync(connection, eventData, result);
    }
}