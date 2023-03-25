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
			("Logout", controllers[0]),
			("ChangePassword", controllers[0]),
			("All", controllers[0]),

			("GetAllByUserId", controllers[1]),
			("Create", controllers[1]),
			("GetById", controllers[1]),
			("Update", controllers[1]),
			("Delete", controllers[1]),
			("All", controllers[1]),

			("GetAll", controllers[2]),
			("GetById", controllers[2]),
			("Update", controllers[2]),
			("Delete", controllers[2]),
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