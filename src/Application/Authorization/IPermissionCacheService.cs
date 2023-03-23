using Domain.Models;

namespace Application.Authorization;

public interface IPermissionCacheService
{
	List<PermissionMappingModel> GetCache();
}