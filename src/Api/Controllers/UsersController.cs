using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Attributes;
using Api.Constants;
using Api.DTOs.User;
using Application.Helpers.Cryptography;
using AutoMapper;
using Domain.Abstractions;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("users")]
[ApiController]
public class UsersController : ControllerBase
{
	private readonly IServiceBase<UserModel> _userService;
	private readonly IHashCryptoHelper _cryptoHelper;
	private readonly IMapper _mapper;

	public UsersController(
		IServiceBase<UserModel> userService,
		IHashCryptoHelper cryptoHelper,
		IMapper mapper)
	{
		_userService = userService;
		_cryptoHelper = cryptoHelper;
		_mapper = mapper;
	}

	[HttpGet]
	[Authorize(Permissions.CheckPermissions)]
	[SessionCheck]
	public async Task<ActionResult<IEnumerable<UserDto>>> GetAllAsync()
	{
		var result = await _userService.GetAllAsync();

		return Ok(_mapper.Map<List<UserDto>>(result));
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<ActionResult<UserDto>> CreateAsync([FromBody] UserCreateDto dto)
	{
		var user = _mapper
			.Map<UserCreateDto, UserModel>(dto, map => map
			.AfterMap((source, destination) =>
				destination.PassHash = _cryptoHelper.GetHashString(source.Password)));

		var result = await _userService.CreateAsync(user);

		return Ok(_mapper.Map<UserDto>(result));
	}

	[HttpGet("{id}")]
	[Authorize(Permissions.CheckPermissions)]
	[SessionCheck]
	public async Task<ActionResult<UserDto>> GetByIdAsync(int id)
	{
		var result = await _userService.GetByIdAsync(id);

		return Ok(_mapper.Map<UserDto>(result));
	}

	[HttpPut]
	[Authorize(Permissions.CheckPermissions)]
	[SessionCheck]
	public async Task<ActionResult<UserDto>> UpdateAsync([FromBody] UserUpdateDto dto)
	{
		var user = _mapper.Map<UserModel>(dto);

		var result = await _userService.UpdateAsync(user);

		return Ok(_mapper.Map<UserDto>(result));
	}

	[HttpDelete]
	[Authorize(Permissions.CheckPermissions)]
	[SessionCheck]
	public async Task<ActionResult> DeleteAsync([FromBody] UserDeleteDto dto)
	{
		var user = _mapper.Map<UserModel>(dto);

		await _userService.DeleteAsync(user);

		return Ok();
	}
}