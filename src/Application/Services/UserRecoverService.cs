using Application.Filters;
using Application.Helpers.Cryptography;
using Application.Messages;
using Application.Models.Login;
using Application.Services.Interfaces;
using Application.Services.Options;
using Application.Validations;
using Domain.Abstractions;
using Domain.Filters;
using Domain.Models;
using Microsoft.Extensions.Options;

namespace Application.Services;

public class UserRecoverService : IUserRecoverService
{
    private readonly IRepositoryBase<UserModel> _userRepository;
    private readonly IHashCryptoHelper _cryptoHelper;
    private readonly IOptionsMonitor<UserSettings> _appSettings;
    private readonly IMessageService _messageService;
    private readonly IUserValidationService _userValidation;
    private readonly IDateTime _dateTime;

    public UserRecoverService(
        IRepositoryBase<UserModel> userDataRepository,
        IHashCryptoHelper hashCryptoHelper,
        IOptionsMonitor<UserSettings> appSettings,
        IMessageService messageService,
        IUserValidationService userValidation,
        IDateTime dateTime)
    {
        _userRepository = userDataRepository;
        _cryptoHelper = hashCryptoHelper;
        _appSettings = appSettings;
        _messageService = messageService;
        _userValidation = userValidation;
        _dateTime = dateTime;
    }

    public UserModel ChangePassword(UserChangePasswordModel model)
    {
        var filter = new UserFilter
        {
            Email = model.Email,
            PassHash = model.OldPassHash
        };

        var user = GetUserByFilter(filter);

        user.PassHash = model.NewPassHash;
        user.FailedLoginCount = 0;

        return _userRepository.Update(user);
    }

    public UserModel ResetPassword(UserResetPasswordModel model)
    {
        var user = GetUserByFilter(new UserFilter { TokenHash = model.Token });

        _userValidation.CheckIfUserPasswordResetTokenIsValid(user);

        user.PassHash = model.NewPassHash;
        user.IsTokenUsed = true;
        user.FailedLoginCount = 0;

        return _userRepository.Update(user);
    }

    public void InitUserPasswordReset(InitPasswordResetModel model)
    {
        var user = GetUserByFilter(new UserFilter { Email = model.Email });

        user.IsTokenUsed = false;
        user.TokenExpirationTime = _dateTime.GetDateTime()
            .AddMinutes(_appSettings.CurrentValue.PasswordResetExpirationInMin);
        user.TokenHash = _cryptoHelper.GetHashString(model.Email);

        var result = _userRepository.Update(user);

        SendPasswordResetConfitmationEmail(result);
    }

    private UserModel GetUserByFilter(IFilter<UserModel> filter)
    {
        var user = _userRepository.GetAll(filter).FirstOrDefault();

        _userValidation.CheckIfUserNotNull(user);

        return user;
    }

    private void SendPasswordResetConfitmationEmail(UserModel model)
    {
        _messageService.SendEmailMessage(
            model.Email,
            ApplicationResource.MessageSubject_PasswordReset,
            string.Format(ApplicationResource.MessageTemplate_PasswordReset, model.TokenHash));
    }
}