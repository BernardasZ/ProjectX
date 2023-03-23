using Application.Database.DbContexts;
using Domain.Filters;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.Database.Repositories;

public class RepositoryBase<TEntity> : IRepository<TEntity>
	where TEntity : ModelBase
{
	private readonly ILogger<RepositoryBase<TEntity>> _logger;
	protected readonly IDbContextBase Context;

	public RepositoryBase(IDbContextBase context, ILogger<RepositoryBase<TEntity>> logger)
	{
		_logger = logger;
		Context = context;
	}

	public virtual List<TEntity> GetAll(IFilter<TEntity> filter = default)
	{
		try
		{
			return GetFilterQuery(filter).ToList();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error while fetching all {typeName}s.", typeof(TEntity).Name);

			throw;
		}
	}

	protected virtual IQueryable<TEntity> GetFilterQuery(IFilter<TEntity> filter = default) => filter != null
			? filter.GetFilter(Context.Set<TEntity>().AsQueryable())
			: Context.Set<TEntity>().AsQueryable();

	public virtual TEntity GetById(int id)
	{
		try
		{
			return Context.Set<TEntity>().Single(x => x.Id == id);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error while getting {typeName}.", typeof(TEntity).Name);

			throw;
		}
	}

	public virtual TEntity Insert(TEntity item)
	{
		try
		{
			var entityEntry = Context.Set<TEntity>().Add(item);

			Save();

			return entityEntry.Entity;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error while inserting {typeName}.", typeof(TEntity).Name);

			throw;
		}
	}

	public virtual TEntity Update(TEntity item)
	{
		try
		{
			var entityEntry = Context.Set<TEntity>().Update(item);
			Save();

			return entityEntry.Entity;
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error while updating {typeName}.", typeof(TEntity).Name);

			throw;
		}
	}

	public virtual void Delete(TEntity entity)
	{
		try
		{
			Context.Set<TEntity>().Remove(entity);
			Save();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error while deleting {typeName}.", typeof(TEntity).Name);

			throw;
		}
	}

	public virtual void Save()
	{
		try
		{
			Context.SaveChanges();
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error while saving changes.");

			throw;
		}
	}
}