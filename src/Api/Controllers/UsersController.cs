using Api.Attributes;
using Api.DTOs.User;
using Application.Helpers.Cryptography;
using Application.Services.Interfaces;
using AutoMapper;
using Domain.Constants;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
	public ActionResult<IEnumerable<UserResponseDto>> GetAll()
	{
		var result = _userService.GetAll();

		return Ok(_mapper.Map<List<UserResponseDto>>(result));
	}

	[HttpPost]
	[AllowAnonymous]
	public ActionResult<UserResponseDto> Create([FromBody] UserCreateDto dto)
	{
		var user = _mapper
			.Map<UserCreateDto, UserModel>(dto, map => map
			.AfterMap((source, destination) =>
				destination.PassHash = _cryptoHelper.GetHashString(source.Password)));

		var result = _userService.Create(user);

		return Ok(_mapper.Map<UserResponseDto>(result));
	}

	[HttpGet("{id}")]
	[Authorize(Permissions.CheckPermissions)]
	[SessionCheck]
	public ActionResult<UserResponseDto> GetById(int id)
	{
		var result = _userService.GetById(id);

		return Ok(_mapper.Map<UserResponseDto>(result));
	}

	[HttpPut]
	[Authorize(Permissions.CheckPermissions)]
	[SessionCheck]
	public ActionResult<UserResponseDto> Update([FromBody] UserUpdateDto dto)
	{
		var user = _mapper.Map<UserModel>(dto);

		var result = _userService.Update(user);

		return Ok(_mapper.Map<UserResponseDto>(result));
	}

	[HttpDelete]
	[Authorize(Permissions.CheckPermissions)]
	[SessionCheck]
	public ActionResult Delete([FromBody] UserDeleteDto dto)
	{
		var user = _mapper.Map<UserModel>(dto);

		_userService.Delete(user);

		return Ok();
	}
}