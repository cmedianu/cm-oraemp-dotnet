using Hides.Domain.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using OraEmp.Application.Services;
using OraEmp.Domain.Entities;
using OraEmp.Infrastructure.Persistence;

namespace OraEmp.Infrastructure.Services;

public class DepartmentService : BaseService<Department>,IDepartmentService
{
    private readonly IDbContextFactory<DataContext> _ctxFactory;

    public DepartmentService(IDbContextFactory<DataContext> ctxFactory) : base(ctxFactory)
    {
        _ctxFactory = ctxFactory;
    }
}