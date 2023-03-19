using Api.Attributes;
using Api.Constants;
using Api.Models.User;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Api.Controllers;

[Route("users")]
[ApiController]
public class UsersController : ControllerBase
{
	private readonly IBaseService<UserModel> _userService;

	public UsersController(IBaseService<UserModel> userService)
	{
		_userService = userService;
	}

	[HttpGet]
	[Authorize(Permissions.CheckPermissions)]
	[SessionCheck]
	public ActionResult<IEnumerable<UserModel>> GetAll() =>
		Ok(_userService.GetAll());

	[HttpPost]
	[AllowAnonymous]
	public IActionResult Create([FromBody] UserCreateModel model)
	{
		var data = new UserModel
		{
			Email = model.Email,
			Name = model.Name,
			Password = model.Password
		};

		_userService.Create(data);

		return Ok();
	}

	[HttpGet("{id}")]
	[Authorize(Permissions.CheckPermissions)]
	[SessionCheck]
	public ActionResult<UserModel> GetById(int id) =>
		Ok(_userService.GetById(id));

	[HttpPut]
	[Authorize(Permissions.CheckPermissions)]
	[SessionCheck]
	public ActionResult<UserModel> Update([FromBody] UserUpdateModel model)
	{
		var data = new UserModel
		{
			Id = model.UserId,
			Email = model.Email,
			Name = model.Name,
		};

		return Ok(_userService.Update(data));
	}

	[HttpDelete]
	[Authorize(Permissions.CheckPermissions)]
	[SessionCheck]
	public ActionResult Delete([FromBody] UserDeleteModel model)
	{
		var data = new UserModel
		{
			Id = model.UserId
		};

		_userService.Delete(data);

		return Ok();
	}
}