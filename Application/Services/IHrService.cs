using OraEmp.Domain.Entities;

namespace OraEmp.Application.Services;

public interface IHrService
{
    public Task<List<Departments>> GetDepartmentList();
}