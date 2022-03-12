using System.Text;

namespace OraEmp.Application.Common;

public class ApplicationState
{
    public ApplicationState(InitialApplicationState init)
    {
        Username = init.Username;
        RemoteIpAddress = init.RemoteIpAddress;
    }

    public string Username { get; }
    public string RemoteIpAddress { get; }


    public override string ToString()
    {
        StringBuilder bld = new(Username + "@" + RemoteIpAddress);
        return bld.ToString();
    }

}