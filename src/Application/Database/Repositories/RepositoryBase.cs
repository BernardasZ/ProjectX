using Application.Database.DbContexts;
using Application.Database.Enums;
using Application.Database.Exceptions;
using Domain.Abstractions;
using Domain.Filters;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Database.Repositories;

public class RepositoryBase<TEntity> : IRepositoryBase<TEntity>
	where TEntity : ModelBase
{
	protected readonly IDbContextBase Context;

	public RepositoryBase(IDbContextBase context) => Context = context;

	public virtual async Task<List<TEntity>> GetAllAsync(IFilter<TEntity> filter = default)
	{
		try
		{
			return await GetFilterQuery(filter).ToListAsync();
		}
		catch (Exception ex)
		{
			throw new RepositoryBaseException(RepositoryErrorCodes.GetAll, $"Error while fetching all {typeof(TEntity).Name}s.", ex);
		}
	}

	protected virtual IQueryable<TEntity> GetFilterQuery(IFilter<TEntity> filter = default) => filter != null
			? filter.GetFilter(Context.Set<TEntity>().AsQueryable())
			: Context.Set<TEntity>().AsQueryable();

	public virtual async Task<TEntity> GetByIdAsync(int id)
	{
		try
		{
			return await Context.Set<TEntity>().SingleAsync(x => x.Id == id);
		}
		catch (Exception ex)
		{
			throw new RepositoryBaseException(RepositoryErrorCodes.GetById, $"Error while getting {typeof(TEntity).Name}.", ex);
		}
	}

	public virtual async Task<TEntity> InsertAsync(TEntity item)
	{
		try
		{
			var entityEntry = await Context.Set<TEntity>().AddAsync(item);

			await SaveAsync();

			return entityEntry.Entity;
		}
		catch (Exception ex)
		{
			throw new RepositoryBaseException(RepositoryErrorCodes.Insert, $"Error while inserting {typeof(TEntity).Name}.", ex);
		}
	}

	public virtual async Task<TEntity> UpdateAsync(TEntity item)
	{
		try
		{
			var entityEntry = Context.Set<TEntity>().Update(item);
			await SaveAsync();

			return entityEntry.Entity;
		}
		catch (Exception ex)
		{
			throw new RepositoryBaseException(RepositoryErrorCodes.Update, $"Error while updating {typeof(TEntity).Name}.", ex);
		}
	}

	public virtual async Task DeleteAsync(TEntity entity)
	{
		try
		{
			Context.Set<TEntity>().Remove(entity);
			await SaveAsync();
		}
		catch (Exception ex)
		{
			throw new RepositoryBaseException(RepositoryErrorCodes.Delete, $"Error while deleting {typeof(TEntity).Name}.", ex);
		}
	}

	public virtual async Task SaveAsync()
	{
		try
		{
			await Context.SaveChangesAsync();
		}
		catch (Exception ex)
		{
			throw new RepositoryBaseException(RepositoryErrorCodes.Save, "Error while saving changes.", ex);
		}
	}
}