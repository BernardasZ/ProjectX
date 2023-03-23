using Domain.Models;

namespace Domain.Filters;

public interface IFilter<TEntity>
	where TEntity : ModelBase
{
	IQueryable<TEntity> GetFilter(IQueryable<TEntity> query);
}