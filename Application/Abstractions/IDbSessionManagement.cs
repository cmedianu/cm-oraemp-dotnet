namespace OraEmp.Infrastructure.Persistence;

public interface IDbSessionManagement
{
    string GetNewSessionId(string userName);
    string[] GetRolesForUser(string userName);
    void RestartSession(string userName,string oldSession);
    void CloseSession(string userName,string oldSession);
}