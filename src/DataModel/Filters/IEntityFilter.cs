using DataModel.Entities.ProjectX;
using System.Linq;

namespace DataModel.Filters;

public interface IEntityFilter<TEntity>
	where TEntity : BaseEntity
{
	IQueryable<TEntity> GetFilter(IQueryable<TEntity> query);
}