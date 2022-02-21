using Microsoft.EntityFrameworkCore;
using OraEmp.Application.Services;
using OraEmp.Domain.Entities;
using OraEmp.Infrastructure.Persistence;

namespace OraEmp.Infrastructure.Services;

public class HrService : IHrService
{
    private readonly IDbContextFactory<OraEmpContext> _ctxFactory;

    public HrService(IDbContextFactory<OraEmpContext> ctxFactory)
    {
        _ctxFactory = ctxFactory;
    }

    public async Task<List<Departments>> GetDepartmentList()
    {
        await using var ctx = await _ctxFactory.CreateDbContextAsync();
        List<Departments> ret = await ctx.Departments.ToListAsync();
        return ret ;
    }
}