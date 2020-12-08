using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoList.Api.Models.User
{
	public class UserLoginResponseModel : UserModel
	{
		public string JWT { get; set; }
	}
}
