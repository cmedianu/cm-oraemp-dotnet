using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using OraEmp.Domain.Entities;

namespace OraEmp.Infrastructure.Persistence;

public interface IDataContext :IDisposable
{
    public DbContext Instance { get; }

    public DbSet<Countries> Countries { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employees> Employees { get; set; }
    public DbSet<JobHistory> JobHistory { get; set; }
    public DbSet<Jobs> Jobs { get; set; }
    public DbSet<Locations> Locations { get; set; }
    public DbSet<Regions> Regions { get; set; }
}