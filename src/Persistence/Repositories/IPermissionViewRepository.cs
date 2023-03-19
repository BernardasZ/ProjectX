using Persistence.Entities.ProjectX;
using Persistence.Filters;
using System.Collections.Generic;

namespace Persistence.Repositories;

public interface IPermissionMappingRepository
{
	List<PermissionMapping> GetAllByFilter(PermissionMappingEntityFilter filter = default);
}