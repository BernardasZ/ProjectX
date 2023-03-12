using DataModel.Entities.ProjectX;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DataModel.Filters;

internal static class FilterHelperExtensions
{
	public static IQueryable<TEntity> WhereIf<TEntity>(this IQueryable<TEntity> query, bool checkCondition, Expression<Func<TEntity, bool>> predicate)
		where TEntity : BaseEntity
	{
		return checkCondition ? query.Where(predicate) : query;
	}
}