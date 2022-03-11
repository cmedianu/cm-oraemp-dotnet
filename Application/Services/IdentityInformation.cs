using OraEmp.Application.Common;
using Serilog;

namespace OraEmp.Application.Services;

public class IdentityInformation
{
    public ApplicationState? State { get; private set; }

    public void setState(InitialApplicationState initial)
    {
        Log.Information("Setting state to {}" , initial);
        State = new ApplicationState(initial);
    }
}