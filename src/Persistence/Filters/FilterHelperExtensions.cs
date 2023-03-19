using System;
using System.Linq;
using System.Linq.Expressions;

namespace Persistence.Filters;

internal static class FilterHelperExtensions
{
	public static IQueryable<TEntity> WhereIf<TEntity>(this IQueryable<TEntity> query, bool checkCondition, Expression<Func<TEntity, bool>> predicate)
		where TEntity : class
	{
		return checkCondition ? query.Where(predicate) : query;
	}
}