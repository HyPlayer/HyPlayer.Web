namespace HyPlayer.Web.Interfaces;

public interface IRepository<TEntity, TId>
{
    Task<bool> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<IQueryable<TEntity>> GetQueryableEntitiesAsync(CancellationToken cancellationToken = default);
}