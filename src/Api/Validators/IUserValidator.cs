namespace Api.Validators
{
	public interface IUserValidator : IBaseValidator<IUserValidator>
	{
		IUserValidator ValidateEmail(string value, string name);

		IUserValidator ValidatePassword(string value, string name);
	}
}