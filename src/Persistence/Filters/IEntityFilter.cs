using Persistence.Entities.ProjectX;
using System.Linq;

namespace Persistence.Filters;

public interface IEntityFilter<TEntity>
	where TEntity : BaseEntity
{
	IQueryable<TEntity> GetFilter(IQueryable<TEntity> query);
}