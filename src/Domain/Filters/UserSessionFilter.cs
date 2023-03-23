using Domain.Extensions;
using Domain.Models;

namespace Domain.Filters;

public class UserSessionFilter : IFilter<UserSessionModel>
{
	public string SessionIdentifier { get; set; }

	public string Ip { get; set; }

	public DateTime? CreateDt { get; set; }

	public int? UserId { get; set; }

	public IQueryable<UserSessionModel> GetFilter(IQueryable<UserSessionModel> query) => query
		.Where(x => SessionIdentifier != null && x.SessionIdentifier == SessionIdentifier)
		.Where(x => Ip != null && x.Ip == Ip)
		.WhereIf(CreateDt != null, x => x.CreateDt == CreateDt)
		.WhereIf(UserId != null, x => x.User.Id == UserId);
}