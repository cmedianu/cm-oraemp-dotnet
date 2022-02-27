using Hides.Domain.Abstractions.Repositories;
using OraEmp.Infrastructure.Persistence;
using Serilog;

namespace OraEmp.Infrastructure.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

public abstract class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
{
    public readonly IDataContext _context;

    public BaseService(IDbContextFactory<DataContext> ctx)
    {
        //Need to resolve the authentication state provider here..
        _context = (IDataContext) ctx.CreateDbContext();
    }

    public void Dispose()
    {
        //_contextFactory.Dispose();
        _context.Dispose();
    }

    // public abstract Task<TEntity> GetByIdAsync(decimal id);
    public async Task<TEntity> GetByIdAsync(decimal id, bool track = true)
    {
        TEntity ret = await _context.Instance.FindAsync<TEntity>(id);
        if (!track)
        {
            _context.Instance.Entry(ret).State = EntityState.Detached;
        }
        return ret;
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _context.Instance.Set<TEntity>().ToListAsync();
    }

    public async Task UpdateAsync(TEntity obj)
    {
        _context.Instance.Update(obj);
        await _context.Instance.SaveChangesAsync();
    }

    public void UpdateNoSave(TEntity obj)
    {
        _context.Instance.Update(obj);
    }

    public async Task SaveChangesAsync()
    {
        await _context.Instance.SaveChangesAsync();
    }

    public async Task InsertAsync(TEntity obj)
    {
        _context.Instance.Add(obj);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity obj)
    {
        _context.Instance.Remove(obj);
        await _context.Instance.SaveChangesAsync();
    }

    public async Task DeleteListAsync(List<TEntity> dbObjList)
    {
        foreach (var entityEntry in _context.Instance.ChangeTracker.Entries())
        {
            entityEntry.State = EntityState.Detached;
            Log.Information(entityEntry.ToString());
        }

        foreach (TEntity entity in dbObjList)
        {
            var entityEntry = _context.Instance.Entry(entity);
            if (entityEntry.IsKeySet)
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                throw new Exception("Cannot delete entity without key" + entity);
            }
        }

        await _context.Instance.SaveChangesAsync();
    }

    public async Task UpsertListAsync(List<TEntity> dbObjList)
    {
        foreach (var entityEntry in _context.Instance.ChangeTracker.Entries())
        {
            entityEntry.State = EntityState.Detached;
            Log.Information(entityEntry.ToString());
        }

        foreach (TEntity entity in dbObjList)
        {
            var entityEntry = _context.Instance.Entry(entity);
            if (entityEntry.IsKeySet)
            {
                entityEntry.State = EntityState.Modified;
            }
            else
            {
                entityEntry.State = EntityState.Added;
                await _context.Instance.SaveChangesAsync();
            }

            entityEntry.CurrentValues.SetValues(entity);
        }

        await _context.Instance.SaveChangesAsync();
    }
}