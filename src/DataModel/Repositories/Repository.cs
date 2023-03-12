using DataModel.DbContexts;
using DataModel.Entities.ProjectX;
using DataModel.Filters;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataModel.Repositories;

internal class Repository<TEntity> : IRepository<TEntity>
	where TEntity : BaseEntity
{
	private readonly ILogger _logger = Log.ForContext<Repository<TEntity>>();
	protected readonly ProjectXDbContext Context;

	public Repository(ProjectXDbContext context)
	{
		Context = context;
	}

	public virtual List<TEntity> GetAllByFilter(IEntityFilter<TEntity> filter = default)
	{
		try
		{
			return GetFilterQuery(filter).ToList();
		}
		catch (Exception ex)
		{
			_logger.Error(ex, "Error while fetching all {typeName}s.", typeof(TEntity).Name);

			throw;
		}
	}

	protected virtual IQueryable<TEntity> GetFilterQuery(IEntityFilter<TEntity> filter = default) => filter != null
			? filter.GetFilter(Context.Set<TEntity>().AsQueryable())
			: Context.Set<TEntity>().AsQueryable();

	public virtual TEntity GetById(int id)
	{
		try
		{
			return Context.Set<TEntity>().Find(id);
		}
		catch (Exception ex)
		{
			_logger.Error(ex, "Error while getting {typeName}.", typeof(TEntity).Name);

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
			_logger.Error(ex, "Error while inserting {typeName}.", typeof(TEntity).Name);

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
			_logger.Error(ex, "Error while updating {typeName}.", typeof(TEntity).Name);

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
			_logger.Error(ex, "Error while deleting {typeName}.", typeof(TEntity).Name);

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
			_logger.Error(ex, "Error while saving changes.");

			throw;
		}
	}
}