namespace Infrastructure.Repository.Common.Interfaces;

public interface IRepository<TEntity> 
    where TEntity : class
{
    Task<List<TEntity>> GetAllEntitiesAsync();

    Task<TEntity> GetSingleEntityBySpecificationAsync(Func<TEntity, bool> predicate);

    Task AddNewEntityAsync(TEntity entity);

    void UpdateExistingEntity(TEntity updatedEntity);

    void RemoveExistingEntity(TEntity removedEntity);
}