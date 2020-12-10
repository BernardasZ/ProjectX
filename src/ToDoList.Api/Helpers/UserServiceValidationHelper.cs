using DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Api.Exeptions;
using UserData = DataModel.Entities.ProjectX.UserData;

namespace ToDoList.Api.Helpers
{
	public interface IUserServiceValidationHelper
	{
		void ValidateUserData(UserData model);
		void ValidateUserId(int userId);
		void ValidateUserPasswordToken(UserData model);
		bool IsAdmin();
	}

	public class UserServiceValidationHelper : IUserServiceValidationHelper
	{
		private readonly IAesCryptoHelper aesCryptoHelper;
		private readonly IClientContextScraper clientContextScraper;
		public UserServiceValidationHelper(
			IAesCryptoHelper aesCryptoHelper,
			IClientContextScraper clientContextScraper)
		{
			this.aesCryptoHelper = aesCryptoHelper;
			this.clientContextScraper = clientContextScraper;
		}

		public void ValidateUserData(UserData model)
		{
			if (model == null)
				throw new GenericException(Enums.GenericErrorEnum.UserDoesNotExist);
		}

		public void ValidateUserId(int userId)
		{
			if (userId > 0 && userId.ToString() != aesCryptoHelper.DecryptString(clientContextScraper.GetClientClaimsIdentityName()))
				throw new GenericException(Enums.GenericErrorEnum.UserIdentityMissMatch);
		}

		public void ValidateUserPasswordToken(UserData model)
		{
			if (model.IsTokenUsed == null || model.IsTokenUsed.Value || model.TokenExpirationTime.Value <= DateTime.Now)
				throw new GenericException(Enums.GenericErrorEnum.UserResetPasswordTokenIsExpired);
		}

		public bool IsAdmin()
		{
			return clientContextScraper.GetClientClaimsRole() == UserRoleEnum.Admin.ToString();
		}
	}
}
