using Persistence.Entities.ProjectX;
using Persistence.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Persistence.Seeds;

internal class RoleSeed
{
	public static IEnumerable<Role> GetRoleSeed() => Enum.GetNames<UserRole>()
		.Select((value, index) => new Role
		{
			Id = index + 1,
			Name = value,
			Value = Enum.Parse<UserRole>(value, true)
		});
}