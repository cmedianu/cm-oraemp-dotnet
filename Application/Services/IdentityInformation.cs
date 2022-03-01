using OraEmp.Application.Common;
using Serilog;

namespace OraEmp.Application.Services;

public class IdentityInformation
{
    public ApplicationState state { get; private set; }

    public void setState(InitialApplicationState initial)
    {
        Log.Information("Setting state to {}" , initial);
        state = new ApplicationState(initial);
    }
}