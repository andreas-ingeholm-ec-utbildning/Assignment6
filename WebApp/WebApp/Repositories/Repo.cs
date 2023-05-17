using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebApp.Contexts;

namespace WebApp.Repositories;

/// <summary>Provides database functionality for <typeparamref name="TEntity"/>.</summary>
public class Repo<TEntity> where TEntity : class
{

    readonly DataContext context;

    public Repo(DataContext context) =>
        this.context = context;

    /// <summary>Adds a <typeparamref name="TEntity"/> to the db.</summary>
    public async Task<TEntity> AddAsync(TEntity entity)
    {
        _ = await context.Set<TEntity>().AddAsync(entity);
        _ = await context.SaveChangesAsync();
        return entity;
    }

    /// <summary>Enumerates all <typeparamref name="TEntity"/> from the db.</summary>
    public async Task<IEnumerable<TEntity>> EnumerateAsync() =>
        await context.Set<TEntity>().ToArrayAsync();

    /// <summary>Gets a <typeparamref name="TEntity"/> from the db.</summary>
    /// <param name="navigationPropertyPath">Include foreign table.</param>
    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate) =>
        await context.Set<TEntity>().FirstOrDefaultAsync(predicate);

    /// <summary>Deletes a <typeparamref name="TEntity"/> from the db.</summary>
    public async Task DeleteAsync(TEntity entity)
    {
        _ = context.Set<TEntity>().Remove(entity);
        _ = await context.SaveChangesAsync();
    }

    /// <summary>Updates a <typeparamref name="TEntity"/> from the db.</summary>
    public async Task UpdateAsync(TEntity entity)
    {
        _ = context.Set<TEntity>().Update(entity);
        _ = await context.SaveChangesAsync();
    }

}
