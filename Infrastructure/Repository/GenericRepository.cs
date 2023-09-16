using Infrastructure.Data.Contexts;
using Infrastructure.Repository.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class GenericRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly ApplicationContext _context;
    
    protected GenericRepository(ApplicationContext dbContext) => 
        _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public Task<List<TEntity>> GetAllEntitiesAsync() =>
         _context.Set<TEntity>().ToListAsync();

    public Task<TEntity> GetSingleEntityBySpecificationAsync(Func<TEntity, bool> predicate) =>
        _context.Set<TEntity>().SingleOrDefaultAsync()!;

    public async Task AddNewEntityAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }
    
    public void UpdateExistingEntity(TEntity updatedEntity)
    {
        _context.Set<TEntity>().Update(updatedEntity);
        _context.SaveChanges();
    }

    public void RemoveExistingEntity(TEntity removedEntity)
    {
        _context.Set<TEntity>().Remove(removedEntity);
        _context.SaveChanges();
    }
}
