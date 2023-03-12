using DataModel.Entities.ProjectX;
using System.Linq;

namespace DataModel.Filters;

public class UserEntityFilter : IEntityFilter<UserData>
{
	public int? Id { get; set; }

	public string UserName { get; set; }

	public string UserEmail { get; set; }

	public string PassHash { get; set; }

	public string TokenHash { get; set; }

	public IQueryable<UserData> GetFilter(IQueryable<UserData> query) => query
		.WhereIf(Id != null, x => x.Id == Id)
		.WhereIf(string.IsNullOrWhiteSpace(UserName), x => x.UserName == UserName)
		.WhereIf(string.IsNullOrWhiteSpace(UserEmail), x => x.UserEmail == UserEmail)
		.WhereIf(string.IsNullOrWhiteSpace(PassHash), x => x.PassHash == PassHash)
		.WhereIf(string.IsNullOrWhiteSpace(TokenHash), x => x.TokenHash == TokenHash);
}