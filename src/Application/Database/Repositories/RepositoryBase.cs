using Application.Database.DbContexts;
using Application.Database.Enums;
using Application.Database.Exceptions;
using Domain.Abstractions;
using Domain.Filters;
using Domain.Models;

namespace Application.Database.Repositories;

public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
	where TEntity : ModelBase
{
	protected readonly IDbContextBase Context;

	public RepositoryBase(IDbContextBase context) => Context = context;

	public virtual List<TEntity> GetAll(IFilter<TEntity> filter = default)
	{
		try
		{
			return GetFilterQuery(filter).ToList();
		}
		catch (Exception ex)
		{
			throw new RepositoryBaseException(RepositoryErrorCodes.GetAll, $"Error while fetching all {typeof(TEntity).Name}s.", ex);
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
			throw new RepositoryBaseException(RepositoryErrorCodes.GetById, $"Error while getting {typeof(TEntity).Name}.", ex);
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
			throw new RepositoryBaseException(RepositoryErrorCodes.Insert, $"Error while inserting {typeof(TEntity).Name}.", ex);
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
			throw new RepositoryBaseException(RepositoryErrorCodes.Update, $"Error while updating {typeof(TEntity).Name}.", ex);
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
			throw new RepositoryBaseException(RepositoryErrorCodes.Delete, $"Error while deleting {typeof(TEntity).Name}.", ex);
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
			throw new RepositoryBaseException(RepositoryErrorCodes.Save, "Error while saving changes.", ex);
		}
	}
}