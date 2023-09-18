namespace Infrastructure.Repository.Common.Interfaces;

public interface IRepository<TEntity> 
    where TEntity : class
{
    Task<List<TEntity>> GetAllEntitiesAsync();

    Task<TEntity?> GetEntityByIdAsync(Guid id);

    Task AddNewEntityAsync(TEntity entity);

    void UpdateExistingEntity(TEntity updatedEntity);

    void RemoveExistingEntity(TEntity removedEntity);
}