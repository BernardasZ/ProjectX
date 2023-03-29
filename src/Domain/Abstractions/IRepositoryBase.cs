using Domain.Filters;
using Domain.Models;

namespace Domain.Abstractions;

public interface IRepositoryBase<TEntity>
	where TEntity : ModelBase
{
	Task<List<TEntity>> GetAllAsync(IFilter<TEntity> filter = default);

	Task<TEntity> GetByIdAsync(int id);

	Task<TEntity> InsertAsync(TEntity entity);

	Task<TEntity> UpdateAsync(TEntity entity);

	Task DeleteAsync(TEntity entity);

	Task SaveAsync();
}