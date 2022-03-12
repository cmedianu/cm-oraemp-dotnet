using OraEmp.Infrastructure.Persistence;
using Xunit;

namespace Infrastructure.UnitTests.Persistence;

public class TestDbSessionManagement
{
    private readonly IDbSessionManagement _svc;

    public TestDbSessionManagement(IDbSessionManagement svc)
    {
        this._svc = svc;
    }

    [Fact]
    public void GetNewSession()
    {
        var newSessionId = _svc.GetNewSessionId("john");
        Assert.NotNull(newSessionId);
    }

    [Fact]
    public void GetRolesForUser()
    {
        var username = "john";
        var roles = _svc.GetRolesForUser(username);
        Assert.Equal(roles.Length,2);
        Assert.Equal(roles[0],username);
    }
}