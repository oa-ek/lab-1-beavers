using BaverGame.Domain.Contracts;
using BaverGame.Persistence.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace BaverGame.Persistence.Repository;

public class GenericRepository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    private readonly ApplicationContext _context;
    
    public GenericRepository(ApplicationContext dbContext) => 
        _context = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public Task<List<TEntity>> GetAllEntitiesAsync() =>
         _context.Set<TEntity>().ToListAsync();

    public async Task<TEntity?> GetEntityByIdAsync(Guid id) =>
        await _context.Set<TEntity>().FindAsync(id);

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
