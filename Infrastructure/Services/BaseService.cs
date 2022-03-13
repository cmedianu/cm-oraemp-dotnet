using OraEmp.Application.Abstractions;
using OraEmp.Infrastructure.Persistence;
using Serilog;

namespace OraEmp.Infrastructure.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public abstract class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
{
    private readonly IDbContextFactory<DataContext> _ctxFactory;
    private readonly ILogger _logger;

    public BaseService(IDbContextFactory<DataContext> ctx, ILogger logger)
    {
        _ctxFactory = ctx;
        _logger = logger?.ForContext<DbSessionManagement>() ?? throw new ArgumentNullException(nameof(logger));
    }

    // public abstract Task<TEntity> GetByIdAsync(decimal id);
    public async Task<TEntity> GetByIdAsync(decimal id, bool track = true)
    {
        using DataContext context = _ctxFactory.CreateDbContext();
        TEntity ret = await context.Instance.FindAsync<TEntity>(id) ?? throw new InvalidOperationException("Could not find object by ID");
        if (!track)
        {
            context.Instance.Entry(ret).State = EntityState.Detached;
        }
        return ret;
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        using DataContext context = _ctxFactory.CreateDbContext();
        return await context.Instance.Set<TEntity>().ToListAsync();
    }

    public async Task UpdateAsync(TEntity obj)
    {
        using DataContext context = _ctxFactory.CreateDbContext();
        context.Instance.Update(obj);
        await context.Instance.SaveChangesAsync();
    }

    public void UpdateNoSave(TEntity obj)
    {
        using DataContext context = _ctxFactory.CreateDbContext();
        context.Instance.Update(obj);
    }

    public async Task SaveChangesAsync()
    {
        using DataContext context = _ctxFactory.CreateDbContext();
        await context.Instance.SaveChangesAsync();
    }

    public async Task InsertAsync(TEntity obj)
    {
        using DataContext context = _ctxFactory.CreateDbContext();
        context.Instance.Add(obj);
        await context.Instance.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity obj)
    {
        using DataContext context = _ctxFactory.CreateDbContext();
        context.Instance.Remove(obj);
        await context.Instance.SaveChangesAsync();
    }

    public async Task DeleteListAsync(List<TEntity> dbObjList)
    {
        using DataContext context = _ctxFactory.CreateDbContext();
        foreach (var entityEntry in context.Instance.ChangeTracker.Entries())
        {
            entityEntry.State = EntityState.Detached;
            _logger.Debug("Deleted Entity {entity}" ,entityEntry.ToString());
        }

        foreach (TEntity entity in dbObjList)
        {
            var entityEntry = context.Instance.Entry(entity);
            if (entityEntry.IsKeySet)
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                throw new Exception("Cannot delete entity without key" + entity);
            }
        }

        await context.Instance.SaveChangesAsync();
    }

    public async Task UpsertListAsync(List<TEntity> dbObjList)
    {
        using DataContext context = _ctxFactory.CreateDbContext();
        foreach (var entityEntry in context.Instance.ChangeTracker.Entries())
        {
            entityEntry.State = EntityState.Detached;
            _logger.Debug("Upserted Entity {entity}" ,entityEntry.ToString());
        }

        foreach (TEntity entity in dbObjList)
        {
            var entityEntry = context.Instance.Entry(entity);
            if (entityEntry.IsKeySet)
            {
                entityEntry.State = EntityState.Modified;
            }
            else
            {
                entityEntry.State = EntityState.Added;
                await context.Instance.SaveChangesAsync();
            }

            entityEntry.CurrentValues.SetValues(entity);
        }

        await context.Instance.SaveChangesAsync();
    }
}