using System.Collections.Generic;
using System.Linq;
using Domain.Models;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Infrastructure.Tests")]

namespace Infrastructure.Databases.ProjectX.Seeds;

internal class PermissionActionSeed
{
	public static IEnumerable<object> GetPermissionActionSeed()
	{
		var controllers = PermissionControllerSeed.GetPermissionControllerSeed().ToList();

		var actionControllerCollection = new List<(string Name, PermissionControllerModel Controller)>
		{
			("LogoutAsync", controllers[0]),
			("ChangePasswordAsync", controllers[0]),
			("CheckSession", controllers[0]),
			("All", controllers[0]),

			("GetAllByUserIdAsync", controllers[1]),
			("CreateAsync", controllers[1]),
			("GetByIdAsync", controllers[1]),
			("UpdateAsync", controllers[1]),
			("DeleteAsync", controllers[1]),
			("All", controllers[1]),

			("GetAllAsync", controllers[2]),
			("GetByIdAsync", controllers[2]),
			("UpdateAsync", controllers[2]),
			("DeleteAsync", controllers[2]),
			("All", controllers[2]),
		};

		return actionControllerCollection
			.Select((action, index) => new
			{
				Id = index + 1,
				action.Name,
				ControllerId = action.Controller.Id.Value
			});
	}
}