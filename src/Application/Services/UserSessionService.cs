using Application.Database.Repositories;
using Application.Filters;
using Application.Services.Interfaces;
using Domain.Models;

namespace Application.Services;

public class UserSessionService : IUserSessionService
{
	private readonly IRepository<UserSessionModel> _userSessionRepository;
	private readonly IClientContextScraper _clientContextScraper;
	private readonly IDateTime _dateTime;
	private readonly ISessionIdentifierEncoder _sessionIdentifierEncoder;

	public UserSessionService(
		IRepository<UserSessionModel> userSessionRepository,
		IClientContextScraper clientContextScraper,
		IDateTime dateTime,
		ISessionIdentifierEncoder sessionIdentifierEncoder)
	{
		_userSessionRepository = userSessionRepository;
		_clientContextScraper = clientContextScraper;
		_dateTime = dateTime;
		_sessionIdentifierEncoder = sessionIdentifierEncoder;
	}

	public bool IsValidUserSession()
	{
		var filter = new UserSessionFilter
		{
			SessionIdentifier = _clientContextScraper.GetClientClaimsIdentityName(),
			Ip = _clientContextScraper.GetClientIpAddress()
		};

		return _userSessionRepository.GetAll(filter).Any();
	}

	public UserSessionModel CreateUserSession(string userId)
	{
		var dateTime = _dateTime.GetDateTime();

		var session = new UserSessionModel
		{
			SessionIdentifier = _sessionIdentifierEncoder.EncodeSessionIdentifier(userId, dateTime),
			Ip = _clientContextScraper.GetClientIpAddress(),
			CreateDt = dateTime
		};

		return _userSessionRepository.Insert(session);
	}

	public void DeleteUserSessionsByIpAndUserId()
	{
		var filter = new UserSessionFilter
		{
			UserId = int.Parse(_sessionIdentifierEncoder.DecodeSessionIdentifier(_clientContextScraper.GetClientClaimsIdentityName()).UserId),
			Ip = _clientContextScraper.GetClientIpAddress()
		};

		DeleteUserSessions(filter);
	}

	public void DeleteUserSessionsByIpAndUserId(int userId)
	{
		var filter = new UserSessionFilter
		{
			UserId = userId,
			Ip = _clientContextScraper.GetClientIpAddress()
		};

		DeleteUserSessions(filter);
	}

	public void DeleteAllUserSessions(int userId)
	{
		var filter = new UserSessionFilter { UserId = userId };

		DeleteUserSessions(filter);
	}

	private void DeleteUserSessions(UserSessionFilter filter)
	{
		var sessions = _userSessionRepository.GetAll(filter);

		sessions.ForEach(session => _userSessionRepository.Delete(session));
	}
}