using Api.Exeptions;
using Persistence.Entities.ProjectX;
using Persistence.Enums;
using System;

namespace Api.Helpers;

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

	public void CheckIfNotNull(User model)
	{
		if (model == null)
		{
			throw new GenericException(Enums.GenericError.UserDoesNotExist);
		}
	}

	public void CheckIfUserIdMatching(int userId)
	{
		if (userId > 0 && userId.ToString() != _aesCryptoHelper.DecryptString(_clientContextScraper.GetClientClaimsIdentityName()))
		{
			throw new GenericException(Enums.GenericError.UserIdentityMissMatch);
		}
	}

	public void CheckIfPasswordResetTokenValid(User model)
	{
		if (model.IsTokenUsed == null
			|| model.IsTokenUsed.Value
			|| model.TokenExpirationTime.Value <= DateTime.Now)
		{
			throw new GenericException(Enums.GenericError.UserResetPasswordTokenIsExpired);
		}
	}

	public bool CheckIfAdminRole() => _clientContextScraper.GetClientClaimsRole() == UserRole.Admin.ToString();
}