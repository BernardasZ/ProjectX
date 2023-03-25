using Domain.Models;

namespace Application.Services.Interfaces;

public interface IUserPermissionService
{
	bool ValidateUserPermissions();

	List<PermissionMappingModel> GetPermissions();
}