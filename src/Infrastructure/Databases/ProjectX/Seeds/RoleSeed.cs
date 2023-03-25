using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Enums;
using Domain.Models;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Infrastructure.Tests")]

namespace Infrastructure.Databases.ProjectX.Seeds;

internal class RoleSeed
{
	public static IEnumerable<RoleModel> GetRoleSeed() =>
		Enum.GetNames<UserRole>()
			.Select((value, index) => new RoleModel
			{
				Id = index + 1,
				Name = value,
				Value = Enum.Parse<UserRole>(value, true)
			});
}