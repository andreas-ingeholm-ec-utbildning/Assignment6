using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebApp.Contexts;

namespace WebApp.Repositories;

public class Repo<TEntity> where TEntity : class
{

    readonly DataContext context;

    public Repo(DataContext context) =>
        this.context = context;

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        _ = await context.Set<TEntity>().AddAsync(entity);
        _ = await context.SaveChangesAsync();
        return entity;
    }

    public async Task<IEnumerable<TEntity>> EnumerateAsync() =>
      await context.Set<TEntity>().ToArrayAsync();

    public async Task<IEnumerable<TEntity>> EnumerateAsync<TProperty>(Expression<Func<TEntity, TProperty>> navigationPropertyPath) =>
      await context.Set<TEntity>().Include(navigationPropertyPath).ToArrayAsync();

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate) =>
        await context.Set<TEntity>().FirstOrDefaultAsync(predicate);

    public async Task DeleteAsync(TEntity entity)
    {
        _ = context.Set<TEntity>().Remove(entity);
        _ = await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _ = context.Set<TEntity>().Update(entity);
        _ = await context.SaveChangesAsync();
    }

}
