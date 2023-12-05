namespace BaverGame.Domain.Contracts;

public interface IRepository<TEntity> 
    where TEntity : class
{
    public List<TEntity> GetAllEntities() =>
        GetAllEntitiesAsync().GetAwaiter().GetResult();
    
    public Task<List<TEntity>> GetAllEntitiesAsync();
    public Task<TEntity?> GetEntityByIdAsync(Guid id);
    public Task AddNewEntityAsync(TEntity entity);
    public void UpdateExistingEntity(TEntity updatedEntity);
    public void RemoveExistingEntity(TEntity removedEntity);
}