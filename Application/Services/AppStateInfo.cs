using OraEmp.Application.Common;
using Serilog;

namespace OraEmp.Application.Services;

public class AppStateInfo : IAppStateInfo
{
    public ApplicationState? State { get; private set; }

    public void SetState(InitialApplicationState initial)
    {
        Log.Information("Setting state to {}" , initial);
        if (State is not null)
        {
            throw new Exception("State already set: Should not happen");
        }
        State = new ApplicationState(initial);
    }
}

