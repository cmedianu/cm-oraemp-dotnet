using Microsoft.EntityFrameworkCore;
using OraEmp.Application.Services;
using OraEmp.Domain.Entities;
using OraEmp.Infrastructure.Persistence;
using Serilog;

namespace OraEmp.Infrastructure.Services;

public class DepartmentService : BaseService<Department>,IDepartmentService
{
    public DepartmentService(IDbContextFactory<DataContext> ctxFactory,ILogger logger) : base(ctxFactory,logger)
    {
    }
}