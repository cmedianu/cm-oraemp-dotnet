using System.Data;
using Oracle.ManagedDataAccess.Client;
using ILogger = Serilog.ILogger;


namespace OraEmp.Infrastructure.Persistence;

public class DbSessionManagement : DbServiceBase, IDbSessionManagement
{
    private readonly ILogger _logger;

    public DbSessionManagement(string connectionString, ILogger logger) : base(connectionString)
    {
        _logger = logger?.ForContext<DbSessionManagement>() ?? throw new ArgumentNullException(nameof(logger));
    }

    public string GetNewSessionId(string userName)
    {
        using (OracleConnection connection = GetConnection())
        {
            OracleCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select dbms_random.string('U', 20) str from dual";
            string sessionId = cmd.ExecuteScalar() as string;
            _logger.Debug("new session: {sessionId}", sessionId);
            return sessionId;
        }
    }

    public string[] GetRolesForUser(string userName)
    {
        using (OracleConnection connection = GetConnection())
        {
            OracleCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select :USER_NAME || ',USER' from dual";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(":USER_NAME", OracleDbType.Varchar2, userName, ParameterDirection.Input);
            string ret = cmd.ExecuteScalar() as string;
            var roles = ret.Split(",");
            _logger.Debug("ROLES: {roles}",roles);
            return roles;
        }
    }

    public void RestartSession(string userName,string oldSession)
    {
        using (OracleConnection connection = GetConnection())
        {
            OracleCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select :OLDSESSION RESTART from dual";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(":OLDSESSION", OracleDbType.Varchar2, userName, ParameterDirection.Input);
            string ret = cmd.ExecuteScalar() as string;
        }
    }

    public void CloseSession(string userName,string oldSession)
    {
        using (OracleConnection connection = GetConnection())
        {
            OracleCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select :OLDSESSION CLOSE from dual";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(":OLDSESSION", OracleDbType.Varchar2, userName, ParameterDirection.Input);
            string ret = cmd.ExecuteScalar() as string;
        }
    }
}