using System.Threading.Tasks;
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
	public async Task<ActionResult<UserLoginResponseDto>> LoginAsync([FromBody] UserLoginDto dto)
	{
		var user = _mapper
			.Map<UserLoginDto, UserLoginModel>(dto, map => map
			.AfterMap((source, destination) =>
				destination.PassHash = _cryptoHelper.GetHashString(source.Password)));

		var result = await _userLoginService.LoginAsync(user);

		return Ok(_mapper.Map<UserLoginResponseDto>(result));
	}

	[HttpDelete("logout")]
	[Authorize(Permissions.CheckPermissions)]
	[SessionCheck]
	public async Task<ActionResult> LogoutAsync()
	{
		await _userLoginService.LogoutAsync();

		return Ok();
	}

	[HttpPost("change-password")]
	[Authorize(Permissions.CheckPermissions)]
	[SessionCheck]
	public async Task<ActionResult<UserDto>> ChangePasswordAsync([FromBody] UserChangePasswordDto dto)
	{
		var user = _mapper
			.Map<UserChangePasswordDto, UserChangePasswordModel>(dto, map => map
			.AfterMap((source, destination) =>
			{
				destination.NewPassHash = _cryptoHelper.GetHashString(source.NewPassword);
				destination.OldPassHash = _cryptoHelper.GetHashString(source.OldPassword);
			}));

		var result = await _userRecoverService.ChangePasswordAsync(user);

		return Ok(_mapper.Map<UserDto>(result));
	}

	[HttpPost("reset-password")]
	[AllowAnonymous]
	public async Task<ActionResult<UserDto>> ResetPasswordAsync([FromBody] UserResetPasswordDto dto)
	{
		var user = _mapper
			.Map<UserResetPasswordDto, UserResetPasswordModel>(dto, map => map
			.AfterMap((source, destination) =>
				destination.NewPassHash = _cryptoHelper.GetHashString(source.NewPassword)));

		var result = await _userRecoverService.ResetPasswordAsync(user);

		return Ok(_mapper.Map<UserDto>(result));
	}

	[HttpPost("init-password-reset")]
	[AllowAnonymous]
	public async Task<ActionResult> InitPasswordResetAsync([FromBody] InitPasswordResetDto dto)
	{
		var user = _mapper.Map<InitPasswordResetModel>(dto);

		await _userRecoverService.InitUserPasswordResetAsync(user);

		return Ok();
	}

	[HttpGet("check-session")]
	[Authorize(Permissions.CheckPermissions)]
	[SessionCheck]
	public ActionResult CheckSession() => Ok();
}