using Persistence.DbContexts;
using Persistence.Entities.ProjectX;
using Persistence.Filters;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Persistence.Repositories;

internal class PermissionMappingRepository : IPermissionMappingRepository
{
	private readonly ILogger _logger = Log.ForContext<PermissionMappingRepository>();
	protected readonly ProjectXDbContext Context;

	public PermissionMappingRepository(ProjectXDbContext context)
	{
		Context = context;
	}

	public virtual List<PermissionMapping> GetAllByFilter(PermissionMappingEntityFilter filter = default)
	{
		try
		{
			return GetFilterQuery(filter).ToList();
		}
		catch (Exception ex)
		{
			_logger.Error(ex, "Error while fetching all {typeName}s.", typeof(PermissionMapping).Name);

			throw;
		}
	}

	protected virtual IQueryable<PermissionMapping> GetFilterQuery(PermissionMappingEntityFilter filter = default) => filter != null
			? filter.GetFilter(Context.Set<PermissionMapping>().AsQueryable())
			: Context.Set<PermissionMapping>().AsQueryable();
}