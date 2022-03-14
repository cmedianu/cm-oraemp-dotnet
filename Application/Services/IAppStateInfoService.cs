using OraEmp.Application.Common;

namespace OraEmp.Application.Services;

public interface IAppStateInfoService
{
    ApplicationState? State { get; }
    void SetState(InitialApplicationState initial);
}