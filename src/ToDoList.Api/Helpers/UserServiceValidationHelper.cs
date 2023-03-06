using DataModel.Entities.ProjectX;
using DataModel.Enums;
using System;
using ToDoList.Api.Exeptions;

namespace ToDoList.Api.Helpers;

public interface IUserServiceValidationHelper
{
	void ValidateUserData(UserData model);

	void ValidateUserId(int userId);

	void ValidateUserPasswordToken(UserData model);

	bool IsAdmin();
}

public class UserServiceValidationHelper : IUserServiceValidationHelper
{
	private readonly IAesCryptoHelper _aesCryptoHelper;
	private readonly IClientContextScraper _clientContextScraper;

	public UserServiceValidationHelper(
		IAesCryptoHelper aesCryptoHelper,
		IClientContextScraper clientContextScraper)
	{
		_aesCryptoHelper = aesCryptoHelper;
		_clientContextScraper = clientContextScraper;
	}

	public void ValidateUserData(UserData model)
	{
		if (model == null)
		{
			throw new GenericException(Enums.GenericErrorEnum.UserDoesNotExist);
		}
	}

	public void ValidateUserId(int userId)
	{
		if (userId > 0 && userId.ToString() != _aesCryptoHelper.DecryptString(_clientContextScraper.GetClientClaimsIdentityName()))
		{
			throw new GenericException(Enums.GenericErrorEnum.UserIdentityMissMatch);
		}
	}

	public void ValidateUserPasswordToken(UserData model)
	{
		if (model.IsTokenUsed == null || model.IsTokenUsed.Value || model.TokenExpirationTime.Value <= DateTime.Now)
		{
			throw new GenericException(Enums.GenericErrorEnum.UserResetPasswordTokenIsExpired);
		}
	}

	public bool IsAdmin() => _clientContextScraper.GetClientClaimsRole() == UserRoleEnum.Admin.ToString();
}
