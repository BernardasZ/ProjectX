using static ToDoList.Api.Constants.ValidationError;

namespace ToDoList.Api.Models.User
{
	public class UserResetPasswordModel
	{
		public string Token { get; set; }
		public string NewPassword { get; set; }
	}
}
