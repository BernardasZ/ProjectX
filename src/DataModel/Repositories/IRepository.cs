using DataModel.Entities.ProjectX;
using DataModel.Filters;
using System.Collections.Generic;

namespace DataModel.Repositories;

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