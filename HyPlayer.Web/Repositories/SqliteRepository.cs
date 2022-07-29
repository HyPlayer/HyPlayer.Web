using HyPlayer.Web.DbContexts;
using HyPlayer.Web.Infrastructure;
using HyPlayer.Web.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HyPlayer.Web.Repositories;

public class SqliteRepository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : class
{
    private readonly SqliteDbContext _dbContext;
    private DbSet<TEntity> Table => _dbContext.Set<TEntity>();

    public SqliteRepository(SqliteDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Table.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<TEntity?> GetById(TId id, CancellationToken cancellationToken = default)
    {
        return await Table.FindAsync(new object?[] { id }, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Table.ToListAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        AttachIfNot(entity);
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Table.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public Task<IQueryable<TEntity>> GetQueryableEntities(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Table.AsQueryable());
    }

    private void AttachIfNot(TEntity entity)
    {
        var entry = _dbContext.ChangeTracker.Entries()
            .FirstOrDefault(ent => ent.Entity == entity);

        if (entry != null)
        {
            return;
        }

        Table.Attach(entity);
    }
}