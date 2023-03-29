using Application.Filters;
using Application.Services.Interfaces;
using Domain.Abstractions;
using Domain.Models;

namespace Application.Services;

public class UserSessionService : IUserSessionService
{
	private readonly IRepositoryBase<UserSessionModel> _userSessionRepository;
	private readonly IClientContextScraper _clientContextScraper;
	private readonly IDateTime _dateTime;
	private readonly ISessionIdentifierEncoder _sessionIdentifierEncoder;

	public UserSessionService(
		IRepositoryBase<UserSessionModel> userSessionRepository,
		IClientContextScraper clientContextScraper,
		IDateTime dateTime,
		ISessionIdentifierEncoder sessionIdentifierEncoder)
	{
		_userSessionRepository = userSessionRepository;
		_clientContextScraper = clientContextScraper;
		_dateTime = dateTime;
		_sessionIdentifierEncoder = sessionIdentifierEncoder;
	}

	public async Task<bool> IsValidUserSessionAsync()
	{
		var filter = new UserSessionFilter
		{
			SessionIdentifier = _clientContextScraper.GetClientClaimsIdentityName(),
			Ip = _clientContextScraper.GetClientIpAddress()
		};

		return (await _userSessionRepository.GetAllAsync(filter)).Any();
	}

	public async Task<UserSessionModel> CreateUserSessionAsync(string userId)
	{
		var dateTime = _dateTime.GetDateTime();

		var session = new UserSessionModel
		{
			SessionIdentifier = _sessionIdentifierEncoder.EncodeSessionIdentifier(userId, dateTime),
			Ip = _clientContextScraper.GetClientIpAddress(),
			CreateDt = dateTime
		};

		return await _userSessionRepository.InsertAsync(session);
	}

	public async Task DeleteUserSessionsByIpAndUserIdAsync()
	{
		var filter = new UserSessionFilter
		{
			UserId = int.Parse(_sessionIdentifierEncoder.DecodeSessionIdentifier(_clientContextScraper.GetClientClaimsIdentityName()).UserId),
			Ip = _clientContextScraper.GetClientIpAddress()
		};

		await DeleteUserSessionsAsync(filter);
	}

	public async Task DeleteUserSessionsByIpAndUserIdAsync(int userId)
	{
		var filter = new UserSessionFilter
		{
			UserId = userId,
			Ip = _clientContextScraper.GetClientIpAddress()
		};

		await DeleteUserSessionsAsync(filter);
	}

	public async Task DeleteAllUserSessionsAsync(int userId)
	{
		var filter = new UserSessionFilter { UserId = userId };

		await DeleteUserSessionsAsync(filter);
	}

	private async Task DeleteUserSessionsAsync(UserSessionFilter filter)
	{
		var sessions = await _userSessionRepository.GetAllAsync(filter);

		foreach (var session in sessions)
		{
			await _userSessionRepository.DeleteAsync(session);
		}
	}
}