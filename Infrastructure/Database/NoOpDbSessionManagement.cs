using System.Data;
using System.Data.Common;
using Oracle.ManagedDataAccess.Client;
using Serilog;

namespace OraEmp.Infrastructure.Persistence;

public class DbSessionManagement : IDbSessionManagement
{
    public DbSessionManagement(string connectionString)
    {
        this.connectionString = connectionString;
    }

    private string connectionString { get; set; }

    private OracleConnection GetConnection()
    {
        var conn = new OracleConnection(connectionString);
        conn.Open();
        conn.ModuleName = this.GetType().Name;
        return conn;
    }

    public string GetNewSessionId(string userName)
    {
        using (OracleConnection connection = GetConnection())
        {
            OracleCommand cmd = connection.CreateCommand();
            cmd.CommandText = "select dbms_random.string('U', 20) str from dual";
            string ret = cmd.ExecuteScalar() as string;
            Log.Debug("new session: {}", ret);
            return ret;
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
            cmd.CommandText = "select :OLDSESSION RESTART from dual";
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add(":OLDSESSION", OracleDbType.Varchar2, userName, ParameterDirection.Input);
            string ret = cmd.ExecuteScalar() as string;
        }
    }
}