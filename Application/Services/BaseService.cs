using Hides.Domain.Abstractions.Repositories;
using OraEmp.Infrastructure.Persistence;

namespace OraEmp.Infrastructure.Services;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using Serilog;

public abstract class BaseService<TEntity> : IBaseService<TEntity> where TEntity : class
{
    public readonly DataContext _context;

    public BaseService(IDbContextFactory<DataContext> ctx)
    {
        //Need to resolve the authentication state provider here..
        _context = ctx.CreateDbContext();
    }

    public void Dispose()
    {
        //_contextFactory.Dispose();
        _context.Dispose();
    }

    // public abstract Task<TEntity> GetByIdAsync(decimal id);
    public async Task<TEntity> GetByIdAsync(decimal id, bool track = true)
    {
        TEntity ret = await _context.FindAsync<TEntity>(id);
        if (!track)
        {
            _context.Entry(ret).State = EntityState.Detached;
        }

        return ret;
    }

    public async Task<List<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>().ToListAsync();
    }

    public async Task UpdateAsync(TEntity obj)
    {
        _context.Update(obj);
        await _context.SaveChangesAsync();
    }

    public void UpdateNoSave(TEntity obj)
    {
        _context.Update(obj);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task InsertAsync(TEntity obj)
    {
        _context.Add(obj);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity obj)
    {
        _context.Remove(obj);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteListAsync(List<TEntity> dbObjList)
    {
        foreach (var entityEntry in _context.ChangeTracker.Entries())
        {
            entityEntry.State = EntityState.Detached;
            Log.Information(entityEntry.ToString());
        }

        foreach (TEntity entity in dbObjList)
        {
            var entityEntry = _context.Entry(entity);
            if (entityEntry.IsKeySet)
            {
                entityEntry.State = EntityState.Deleted;
            }
            else
            {
                throw new Exception("Cannot delete entity without key" + entity);
            }
        }

        await _context.SaveChangesAsync();
    }

    public async Task UpsertListAsync(List<TEntity> dbObjList)
    {
        foreach (var entityEntry in _context.ChangeTracker.Entries())
        {
            entityEntry.State = EntityState.Detached;
            Log.Information(entityEntry.ToString());
        }

        foreach (TEntity entity in dbObjList)
        {
            var entityEntry = _context.Entry(entity);
            if (entityEntry.IsKeySet)
            {
                entityEntry.State = EntityState.Modified;
            }
            else
            {
                entityEntry.State = EntityState.Added;
                await _context.SaveChangesAsync();
            }

            entityEntry.CurrentValues.SetValues(entity);
        }

        await _context.SaveChangesAsync();
    }
}