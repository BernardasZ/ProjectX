using System.Collections.Generic;
using System.Linq;
using Infrastructure.Helpers;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Infrastructure.Tests")]

namespace Infrastructure.Databases.ProjectX.Seeds;

internal class PermissionMappingSeed
{
	public static IEnumerable<object> GetPermissionMappingSeed() =>
		PermissionActionSeed.GetPermissionActionSeed()
			.Where(action => ReflectionHelper.GetPropertyValue<string>(action, "Name").Equals("All"))
			.Select((action, index) => new
			{
				Id = index + 1,
				AllowAllActions = true,
				ActionId = ReflectionHelper.GetPropertyValue<int>(action, "Id"),
				ControllerId = ReflectionHelper.GetPropertyValue<int>(action, "ControllerId"),
				RoleId = 1
			});
}