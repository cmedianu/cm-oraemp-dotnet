using OraEmp.Application.Common;

namespace OraEmp.Application.Services;

public interface IAppStateInfo
{
    ApplicationState? State { get; }
    void SetState(InitialApplicationState initial);
}