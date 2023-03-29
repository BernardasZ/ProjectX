using System.Text.RegularExpressions;

namespace Domain.Validators;

public class UserValidator : BaseValidator<IUserValidator>, IUserValidator
{
	private const string _emailRegex = @"^[a-z0-9][-a-z0-9.!#$%&'*+-=?^_`{|}~\/]+@([-a-z0-9]+\.)+[a-z]{2,5}$";

	public IUserValidator ValidateEmail(string value, string name)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			AddDefaultMessage(name);		
		}
		else if (!new Regex(_emailRegex).IsMatch(value))
		{
			AddCustomMessage("Incorrect email format.", name);
		}

		return GetValidator();
	}

	public IUserValidator ValidatePassword(string value, string name)
	{
		if (string.IsNullOrWhiteSpace(value))
		{
			AddDefaultMessage(name);
		}
		else if (value.Length < 12)
		{
			AddCustomMessage("Password is too short.", name);
		}

		return GetValidator();
	}
}