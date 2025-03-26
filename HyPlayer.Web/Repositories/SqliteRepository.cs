using HyPlayer.Web.DbContexts;
using HyPlayer.Web.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HyPlayer.Web.Repositories;

public class SqliteRepository<TEntity, TId>(SqliteDbContext dbContext) : IRepository<TEntity, TId>
    where TEntity : class
{
    private DbSet<TEntity> Table => dbContext.Set<TEntity>();

    public async Task<bool> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Table.AddAsync(entity, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await Table.FindAsync([id], cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Table.ToListAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        AttachIfNot(entity);
        dbContext.Entry(entity).State = EntityState.Modified;
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Table.Remove(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public Task<IQueryable<TEntity>> GetQueryableEntitiesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Table.AsQueryable());
    }

    private void AttachIfNot(TEntity entity)
    {
        var entry = dbContext.ChangeTracker.Entries()
            .FirstOrDefault(ent => ent.Entity == entity);

        if (entry != null)
        {
            return;
        }

        Table.Attach(entity);
    }
}