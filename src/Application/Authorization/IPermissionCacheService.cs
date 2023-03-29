using Domain.Models;

namespace Application.Authorization;

public interface IPermissionCacheService
{
	Task<List<PermissionMappingModel>> GetCacheAsync();
}