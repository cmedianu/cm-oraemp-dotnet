using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hides.Domain.Abstractions.Repositories
{
    public interface IBaseService<TEntity> : IDisposable
    {
        public Task<TEntity> GetByIdAsync(decimal id, bool track = true);

        public Task<List<TEntity>> GetAllAsync();

        public Task UpdateAsync(TEntity obj);

        public Task InsertAsync(TEntity obj);

        public Task DeleteAsync(TEntity obj);
        public Task UpsertListAsync(List<TEntity> dbObjList);
        public Task DeleteListAsync(List<TEntity> dbObjList);
        public  void UpdateNoSave(TEntity obj);
        public Task SaveChangesAsync();
    }
}