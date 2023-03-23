using Domain.Models;
using System.Collections.Generic;

namespace Api.Services.Interfaces;

public interface IUserPermissionService
{
	bool ValidateUserPermissions();

	List<PermissionMappingModel> GetPermissions();
}