using Persistence.Entities.ProjectX;
using System.Linq;

namespace Persistence.Filters;

public class UserEntityFilter : IEntityFilter<User>
{
	public int? Id { get; set; }

	public string Name { get; set; }

	public string Email { get; set; }

	public string PassHash { get; set; }

	public string TokenHash { get; set; }

	public IQueryable<User> GetFilter(IQueryable<User> query) => query
		.WhereIf(Id != null, x => x.Id == Id)
		.WhereIf(!string.IsNullOrWhiteSpace(Name), x => x.Name == Name)
		.WhereIf(!string.IsNullOrWhiteSpace(Email), x => x.Email == Email)
		.WhereIf(!string.IsNullOrWhiteSpace(PassHash), x => x.PassHash == PassHash)
		.WhereIf(!string.IsNullOrWhiteSpace(TokenHash), x => x.TokenHash == TokenHash);
}