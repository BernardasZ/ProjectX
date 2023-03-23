using Application.Extensions;
using Domain.Filters;
using Domain.Models;

namespace Application.Filters;

public class UserSessionFilter : IFilter<UserSessionModel>
{
	public string SessionIdentifier { get; set; }

	public string Ip { get; set; }

	public DateTime? CreateDt { get; set; }

	public int? UserId { get; set; }

	public IQueryable<UserSessionModel> GetFilter(IQueryable<UserSessionModel> query) => query
		.WhereIf(SessionIdentifier != null, x => x.SessionIdentifier == SessionIdentifier)
		.WhereIf(Ip != null, x => x.Ip == Ip)
		.WhereIf(CreateDt.HasValue, x => x.CreateDt == CreateDt)
		.WhereIf(UserId.HasValue, x => x.User.Id == UserId);
}