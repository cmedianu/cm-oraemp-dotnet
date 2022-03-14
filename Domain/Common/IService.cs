namespace OraEmp.Domain.Common
{
    public interface IService<TEntity>
    {
        Task<TEntity> GetByIdAsync(decimal id);
        Task<List<TEntity>> GetAllAsync();
    }
}