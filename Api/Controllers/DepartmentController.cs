using Microsoft.AspNetCore.Mvc;
using OraEmp.Application.Services;
using OraEmp.Domain.Entities;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentService _service;

    public DepartmentController(IDepartmentService service)
    {
        _service = service;
    }

    [HttpGet(Name = "GetDepartments")]
    public async Task<IEnumerable<Department>> Get()
    {
        return await _service.GetAllAsync();
    }
}