using Persistence.Entities.ProjectX;
using System.Collections.Generic;

namespace Api.Services;

public interface IPermissionCacheService
{
	List<PermissionMapping> GetCache();
}