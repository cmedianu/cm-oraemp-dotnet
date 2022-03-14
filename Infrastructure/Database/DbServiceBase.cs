using Oracle.ManagedDataAccess.Client;

namespace OraEmp.Infrastructure.Persistence;

public class DbServiceBase
{
    public DbServiceBase(string connectionString)
    {
        this.connectionString = connectionString;
    }

    private string connectionString { get; set; }

    protected OracleConnection GetConnection()
    {
        var conn = new OracleConnection(connectionString);
        conn.Open();
        conn.ModuleName = this.GetType().Name;
        return conn;
    }
}