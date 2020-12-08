using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static ToDoList.Api.Constants.ValidationError;

namespace ToDoList.Api.Models.User
{
	public class UserLoginModel
	{
		public string UserEmail { get; set; }
		public string Password { get; set; }
	}
}
