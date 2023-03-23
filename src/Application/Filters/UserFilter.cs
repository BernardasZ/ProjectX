using Application.Extensions;
using Domain.Filters;
using Domain.Models;

namespace Application.Filters;

public class UserFilter : IFilter<UserModel>
{
	public int? Id { get; set; }

	public string Name { get; set; }

	public string Email { get; set; }

	public string PassHash { get; set; }

	public string TokenHash { get; set; }

	public IQueryable<UserModel> GetFilter(IQueryable<UserModel> query) => query
		.WhereIf(Id != null, x => x.Id == Id)
		.WhereIf(!string.IsNullOrWhiteSpace(Name), x => x.Name == Name)
		.WhereIf(!string.IsNullOrWhiteSpace(Email), x => x.Email == Email)
		.WhereIf(!string.IsNullOrWhiteSpace(PassHash), x => x.PassHash == PassHash)
		.WhereIf(!string.IsNullOrWhiteSpace(TokenHash), x => x.TokenHash == TokenHash);
}