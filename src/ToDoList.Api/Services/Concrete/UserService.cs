using DataModel.Entities.ProjectX;
using DataModel.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoList.Api.Helpers;

namespace ToDoList.Api.Services.Concrete
{
	public class UserService : IUserService
	{
		private readonly IRepository<UserSession> userSessionRepository;
		private readonly IClientContextScraper clientContextScraper;

		public UserService(
			IRepository<UserSession> userSessionRepository,
			IClientContextScraper clientContextScraper)
		{
			this.userSessionRepository = userSessionRepository;
			this.clientContextScraper = clientContextScraper;
		}

		public bool IsValidUserSession()
		{
			string userIdentity = clientContextScraper.GetClientClaimsName();
			string ipAddress = clientContextScraper.GetClientIpAddress();

			return userSessionRepository.FetchAll().Where(x => x.SessionIdentifier == userIdentity && x.Ip == ipAddress).Any();
		}
	}
}
