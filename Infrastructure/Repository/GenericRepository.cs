using Infrastructure.Data.Contexts;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class GenericRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    protected GenericRepository(ApplicationContext dbContext) => 
        Context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    private ApplicationContext Context { get; init; }

    public async Task<IEnumerable<TEntity>> GetAllEntitiesAsync() =>
        await Context.Set<TEntity>().ToListAsync();

    public async Task<TEntity> GetSingleEntityBySpecificationAsync(Func<TEntity, bool> predicate) =>
        (await Context.Set<TEntity>().SingleOrDefaultAsync())!;

    public async Task AddNewEntityAsync(TEntity entity)
    {
        await Context.Set<TEntity>().AddAsync(entity);
        await Context.SaveChangesAsync();
    }
    
    public void UpdateExistingEntity(TEntity updatedEntity)
    {
        Context.Set<TEntity>().Update(updatedEntity);
        Context.SaveChanges();
    }

    public virtual void RemoveExistingEntity(TEntity removedEntity)
    {
        Context.Set<TEntity>().Remove(removedEntity);
        Context.SaveChanges();
    }
}
