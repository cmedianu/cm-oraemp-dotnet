using System.Text;

namespace OraEmp.Application.Common;

public class ApplicationState
{
    public ApplicationState(InitialApplicationState init)
    {
        Username = init.Username;
        RemoteIpAddress = init.RemoteIpAddress;
    }

    private string Username { get; }
    private string RemoteIpAddress { get; }

    private string? LoggedInUser { get; set; }


    public override string ToString()
    {
        StringBuilder bld = new(Username + "@" + RemoteIpAddress);
        return bld.ToString();
    }

    public void SetLoggedInUser(string modelLoginUser)
    {
        LoggedInUser = modelLoginUser;
    }
}