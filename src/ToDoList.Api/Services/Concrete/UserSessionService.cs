using DataModel.Entities.ProjectX;
using DataModel.Filters;
using DataModel.Repositories;
using System;
using System.Linq;
using ToDoList.Api.Exeptions;
using ToDoList.Api.Helpers;

namespace ToDoList.Api.Services.Concrete;

public class UserSessionService : IUserSessionService
{
	private readonly IRepository<UserSession> _userSessionRepository;
	private readonly IClientContextScraper _clientContextScraper;
	private readonly IAesCryptoHelper _aesCryptoHelper;

	public UserSessionService(
		IRepository<UserSession> userSessionRepository,
		IClientContextScraper clientContextScraper,
		IAesCryptoHelper aesCryptoHelper)
	{
		_userSessionRepository = userSessionRepository;
		_clientContextScraper = clientContextScraper;
		_aesCryptoHelper = aesCryptoHelper;
	}

	public bool IsValidUserSession() => _userSessionRepository
			.GetAllByFilter(GetUserSessionFilter())
			.Any();

	public void CreateUserSession(string userId)
	{
		var session = new UserSession
		{
			SessionIdentifier = _aesCryptoHelper.EncryptString(userId),
			Ip = _clientContextScraper.GetClientIpAddress(),
			CreateDt = DateTime.Now
		};

		_userSessionRepository.Insert(session);
	}

	public void DeleteUserSession()
	{
		var session = _userSessionRepository
			.GetAllByFilter(GetUserSessionFilter())
			.FirstOrDefault();

		if (session == null)
		{
			throw new GenericException("Session does not exist.");
		}

		_userSessionRepository.Delete(session);
	}

	public void DeleteUserSession(string userId)
	{
		var session = _userSessionRepository
			.GetAllByFilter(GetUserSessionFilterByUserId(userId))
			.FirstOrDefault();

		if (session == null)
		{
			throw new GenericException("Session does not exist.");
		}

		_userSessionRepository.Delete(session);
	}

	private UserSessionEntityFilter GetUserSessionFilter() => new()
	{
		SessionIdentifier = _clientContextScraper.GetClientClaimsIdentityName(),
		Ip = _clientContextScraper.GetClientIpAddress()
	};

	private UserSessionEntityFilter GetUserSessionFilterByUserId(string userId) => new()
	{
		SessionIdentifier = _aesCryptoHelper.EncryptString(userId),
		Ip = _clientContextScraper.GetClientIpAddress()
	};
}