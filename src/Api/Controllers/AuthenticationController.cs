using Api.Attributes;
using Api.Constants;
using Api.DTOs.Login;
using Api.DTOs.User;
using Application.Helpers.Cryptography;
using Application.Models.Login;
using Application.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
	private readonly IUserLoginService _userLoginService;
	private readonly IUserRecoverService _userRecoverService;
	private readonly IHashCryptoHelper _cryptoHelper;
	private readonly IMapper _mapper;

	public AuthenticationController(
		IUserLoginService userLoginService,
		IUserRecoverService userRecoverService,
		IHashCryptoHelper cryptoHelper,
		IMapper mapper)
	{
		_userLoginService = userLoginService;
		_userRecoverService = userRecoverService;
		_cryptoHelper = cryptoHelper;
		_mapper = mapper;
	}

	[HttpPost("login")]
	[AllowAnonymous]
	public ActionResult<UserLoginResponseDto> Login([FromBody] UserLoginDto dto)
	{
		var user = _mapper
			.Map<UserLoginDto, UserLoginModel>(dto, map => map
			.AfterMap((source, destination) =>
				destination.PassHash = _cryptoHelper.GetHashString(source.Password)));

		var result = _userLoginService.Login(user);

		return Ok(_mapper.Map<UserLoginResponseDto>(result));
	}

	[HttpPost("logout")]
	[Authorize(Permissions.CheckPermissions)]
	[SessionCheck]
	public ActionResult Logout()
	{
		_userLoginService.Logout();

		return Ok();
	}

	[HttpPost("change-password")]
	[Authorize(Permissions.CheckPermissions)]
	[SessionCheck]
	public ActionResult<UserResponseDto> ChangePassword([FromBody] UserChangePasswordDto dto)
	{
		var user = _mapper
			.Map<UserChangePasswordDto, UserChangePasswordModel>(dto, map => map
			.AfterMap((source, destination) =>
			{
				destination.NewPassHash = _cryptoHelper.GetHashString(source.NewPassword);
				destination.OldPassHash = _cryptoHelper.GetHashString(source.OldPassword);
			}));

		var result = _userRecoverService.ChangePassword(user);

		return Ok(_mapper.Map<UserResponseDto>(result));
	}

	[HttpPost("reset-password")]
	[AllowAnonymous]
	public ActionResult<UserResponseDto> ResetPassword([FromBody] UserResetPasswordDto dto)
	{
		var user = _mapper
			.Map<UserResetPasswordDto, UserResetPasswordModel>(dto, map => map
			.AfterMap((source, destination) =>
				destination.NewPassHash = _cryptoHelper.GetHashString(source.NewPassword)));

		var result = _userRecoverService.ResetPassword(user);

		return Ok(_mapper.Map<UserResponseDto>(result));
	}

	[HttpPost("init-password-reset")]
	[AllowAnonymous]
	public IActionResult InitPasswordReset([FromBody] InitPasswordResetDto dto)
	{
		var user = _mapper.Map<InitPasswordResetModel>(dto);

		_userRecoverService.InitUserPasswordReset(user);

		return Ok();
	}
}