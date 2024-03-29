﻿using Domain.Enums;
using Domain.Validators;

namespace Api.DTOs.User;

public class UserDto : BaseValidatableObject
{
	public int Id { get; set; }

	public string Name { get; set; }

	public string Email { get; set; }

	public UserRole Role { get; set; }

	protected override IBaseValidator<IUserValidator> Validate() => new UserValidator()
		.ValidateId(Id, nameof(Id))
		.ValidateString(Name, nameof(Name))
		.ValidateEmail(Email, nameof(Email));
}