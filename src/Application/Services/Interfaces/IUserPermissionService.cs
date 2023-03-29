using Domain.Models;

namespace Application.Services.Interfaces;

public interface IUserPermissionService
{
	Task<bool> ValidateUserPermissionsAsync();

	Task<List<PermissionMappingModel>> GetPermissionsAsync();
}