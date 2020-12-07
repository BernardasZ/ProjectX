using DataModel.Entities.ProjectX;
using DataModel.Repositories;
using System;
using System.Linq;
using ToDoList.Api.Helpers;

namespace ToDoList.Api.Services.Concrete
{
	public class UserSessionService : IUserSessionService
	{
		private readonly IRepository<UserSession> userSessionRepository;
		private readonly IClientContextScraper clientContextScraper;
		private readonly IAesCryptoHelper aesCryptoHelper;

		public UserSessionService(
			IRepository<UserSession> userSessionRepository,
			IClientContextScraper clientContextScraper,
			IAesCryptoHelper aesCryptoHelper)
		{
			this.userSessionRepository = userSessionRepository;
			this.clientContextScraper = clientContextScraper;
			this.aesCryptoHelper = aesCryptoHelper;
		}

		public bool IsValidUserSession()
		{
			string userIdentity = clientContextScraper.GetClientClaimsName();
			string ipAddress = clientContextScraper.GetClientIpAddress();

			return userSessionRepository.FetchAll().Where(x => x.SessionIdentifier == userIdentity && x.Ip == ipAddress).Any();
		}

		public void CreateUserSession(string userId)
		{
			string ipAddress = clientContextScraper.GetClientIpAddress();
			string userIdentity = aesCryptoHelper.EncryptString(userId);

			UserSession userSession = new UserSession()
			{
				SessionIdentifier = userIdentity,
				Ip = ipAddress,
				CreateDt = DateTime.Now
			};

			userSessionRepository.Insert(userSession);
			userSessionRepository.Save();
		}

		public void DeleteUserSession()
		{
			string ipAddress = clientContextScraper.GetClientIpAddress();
			string userIdentity = clientContextScraper.GetClientClaimsName();

			DeleteUserSession(userIdentity, ipAddress);
		}

		public void DeleteUserSession(string userId)
		{
			string ipAddress = clientContextScraper.GetClientIpAddress();
			string userIdentity = aesCryptoHelper.EncryptString(userId);

			DeleteUserSession(userIdentity, ipAddress);
		}

		public void DeleteUserSession(string userIdentity, string ipAddress)
		{
			var session = userSessionRepository.FetchAll().Where(x => x.SessionIdentifier == userIdentity && x.Ip == ipAddress).FirstOrDefault();

			if (session != null)
			{
				userSessionRepository.Delete(session);
				userSessionRepository.Save();
			}
		}
	}
}
