﻿using DataModel.Entities.ProjectX;
using DataModel.Repositories;
using System;
using System.Linq;
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

	public bool IsValidUserSession()
	{
		string userIdentity = _clientContextScraper.GetClientClaimsIdentityName();
		string ipAddress = _clientContextScraper.GetClientIpAddress();

		return _userSessionRepository
			.FetchAll()
			.Any(x => x.SessionIdentifier == userIdentity && x.Ip == ipAddress);
	}

	public void CreateUserSession(string userId)
	{
		string ipAddress = _clientContextScraper.GetClientIpAddress();
		string userIdentity = _aesCryptoHelper.EncryptString(userId);

		UserSession userSession = new UserSession
		{
			SessionIdentifier = userIdentity,
			Ip = ipAddress,
			CreateDt = DateTime.Now
		};

		_userSessionRepository.Insert(userSession);
		_userSessionRepository.Save();
	}

	public void DeleteUserSession()
	{
		string ipAddress = _clientContextScraper.GetClientIpAddress();
		string userIdentity = _clientContextScraper.GetClientClaimsIdentityName();

		DeleteUserSession(userIdentity, ipAddress);
	}

	public void DeleteUserSession(string userId)
	{
		string ipAddress = _clientContextScraper.GetClientIpAddress();
		string userIdentity = _aesCryptoHelper.EncryptString(userId);

		DeleteUserSession(userIdentity, ipAddress);
	}

	public void DeleteUserSession(string userIdentity, string ipAddress)
	{
		var session = _userSessionRepository
			.FetchAll()
			.FirstOrDefault(x => x.SessionIdentifier == userIdentity && x.Ip == ipAddress);

		if (session != null)
		{
			_userSessionRepository.Delete(session);
			_userSessionRepository.Save();
		}
	}
}
