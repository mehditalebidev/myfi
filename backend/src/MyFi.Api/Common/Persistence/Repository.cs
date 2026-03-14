using Microsoft.EntityFrameworkCore;

namespace MyFi.Api.Common.Persistence;

public sealed class Repository : IRepository
{
    public Repository(MyFiDbContext dbContext)
    {
        DbContext = dbContext;
    }

    public MyFiDbContext DbContext { get; }

    public IQueryable<TEntity> Query<TEntity>() where TEntity : class
    {
        return DbContext.Set<TEntity>();
    }

    public Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class
    {
        return DbContext.Set<TEntity>().AddAsync(entity, cancellationToken).AsTask();
    }

    public void Remove<TEntity>(TEntity entity) where TEntity : class
    {
        DbContext.Set<TEntity>().Remove(entity);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        return DbContext.SaveChangesAsync(cancellationToken);
    }
}
