using System.Collections.Generic;
using System.Linq;
using Domain.Models;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Infrastructure.Tests")]

namespace Infrastructure.Databases.ProjectX.Seeds;

internal class PermissionControllerSeed
{
	public static IEnumerable<PermissionControllerModel> GetPermissionControllerSeed() =>
		new List<string> { "Authentication", "Tasks", "Users" }
			.Select((controller, index) => new PermissionControllerModel
			{
				Id = index + 1,
				Name = controller
			});
}