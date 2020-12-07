using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static ToDoList.Api.Constants.ValidationError;

namespace ToDoList.Api.Models
{
	public class LoginModel
	{
		[Required(ErrorMessage = UserCredentialRequired)]
		public string UserEmail { get; set; }

		[Required(ErrorMessage = PasswordRequired)]
		[MinLength(12, ErrorMessage = PasswordLength)]
		public string Password { get; set; }
	}
}
