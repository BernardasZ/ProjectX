[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Infrastructure.Tests")]

namespace Infrastructure.Databases.ProjectX.Seeds;

internal class UserSeed
{
	public static object GetUserSeed() => new
	{
		Id = 1,
		Name = "projectxadmin",
		Email = "admin@projectx.com",
		PassHash = "E5scnWql/WJsaL0tYvsNKVbYP8ZJR0s0WNNhCjLlcXw=",
		FailedLoginCount = 0,
		RoleId = 1
	};
}