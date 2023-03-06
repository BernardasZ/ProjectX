using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ToDoList.Api.Validators;

public class UserValidator : BaseValidator
{
    private readonly string _emailRegex = @"^[a-z0-9][-a-z0-9.!#$%&'*+-=?^_`{|}~\/]+@([-a-z0-9]+\.)+[a-z]{2,5}$";

    public T ValidateEmail<T>(string value, string name) where T : BaseValidator
    {
        if (string.IsNullOrWhiteSpace(value) || !new Regex(_emailRegex).IsMatch(value))
        {
            AddDefaultMessage(name);
        }

        return this as T;
    }

	public T ValidatePassword<T>(string value, string name) where T : BaseValidator
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            AddDefaultMessage(name);
        }
        else if (value.Length < 12)
        {
            AddCustomMessage($"Invalid property \"{name}\", is too short.");
        }

        return this as T;
    }
}
