using Application.Services.Interfaces;
using Domain.Enums;
using Domain.Exeptions;
using Domain.Models;

namespace Application.Validations;

public class UserValidationService : IUserValidationService
{
	private readonly IClientContextScraper _clientContextScraper;
	private readonly ISessionIdentifierEncoder _sessionIdentifierEncoder;
	private readonly IDateTime _dateTime;

	public UserValidationService(
		IClientContextScraper clientContextScraper,
		IDateTime dateTime,
		ISessionIdentifierEncoder sessionIdentifierEncoder)
	{
		_clientContextScraper = clientContextScraper;
		_dateTime = dateTime;
		_sessionIdentifierEncoder = sessionIdentifierEncoder;
	}

	public void CheckIfUserNotNull(UserModel model)
	{
		if (model == null)
		{
			throw new GenericException(GenericError.UserDoesNotExist);
		}
	}

	public void CheckIfUserIdMatchesSessionId(int userId)
	{
		var sessionIdentifier =
			_sessionIdentifierEncoder.DecodeSessionIdentifier(
				_clientContextScraper.GetClientClaimsIdentityName());

		if (userId > 0 && userId.ToString() != sessionIdentifier.UserId)
		{
			throw new GenericException(GenericError.UserIdentityMissMatch);
		}
	}

	public void CheckIfUserPasswordResetTokenIsValid(UserModel model)
	{
		if (model.IsTokenUsed == null
			|| model.IsTokenUsed.Value
			|| model.TokenExpirationTime.Value <= _dateTime.GetDateTime())
		{
			throw new GenericException(GenericError.UserResetPasswordTokenIsExpired);
		}
	}

	public bool CheckIfUserIsAdmin() => _clientContextScraper.GetClientClaimsRole() == UserRole.Admin.ToString();
}