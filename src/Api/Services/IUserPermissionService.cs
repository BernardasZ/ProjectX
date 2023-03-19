using Persistence.Entities.ProjectX;
using System.Collections.Generic;

namespace Api.Services;

public interface IUserPermissionService
{
	bool ValidateUserPermissions();

	List<PermissionMapping> GetPermissions();
}