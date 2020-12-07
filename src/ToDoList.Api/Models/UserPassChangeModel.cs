using System.ComponentModel.DataAnnotations;
using static ToDoList.Api.Constants.ValidationError;

namespace ToDoList.Api.Models
{
	public class UserPassChangeModel : UserModel
	{
		[Required(ErrorMessage = PasswordRequired)]
		[MinLength(12, ErrorMessage = PasswordLength)]
		public string NewPassword { get; set; }
	}
}
