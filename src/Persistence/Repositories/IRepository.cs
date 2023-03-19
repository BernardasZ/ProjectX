using Persistence.Entities.ProjectX;
using Persistence.Filters;
using System.Collections.Generic;

namespace Persistence.Repositories;

public interface IRepository<TEntity>
	where TEntity : BaseEntity
{
	List<TEntity> GetAllByFilter(IEntityFilter<TEntity> filter = default);

	TEntity GetById(int id);

	TEntity Insert(TEntity entity);

	TEntity Update(TEntity entity);

	void Delete(TEntity entity);

	void Save();
}