using System.Collections.Generic;
using System.Linq;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Infrastructure.Tests")]

namespace Infrastructure.Databases.ProjectX.Seeds;

internal class PermissionMappingSeed
{
	public static IEnumerable<object> GetPermissionMappingSeed() =>
		PermissionActionSeed.GetPermissionActionSeed()
			.Where(action => action.GetType().GetProperty("Name").GetValue(action).Equals("All"))
			.Select((action, index) => new
			{
				Id = index + 1,
				AllowAllActions = true,
				ActionId = action.GetType().GetProperty("Id").GetValue(action),
				ControllerId = action.GetType().GetProperty("ControllerId").GetValue(action),
				RoleId = 1
			});
}