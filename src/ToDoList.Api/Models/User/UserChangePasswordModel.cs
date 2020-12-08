using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static ToDoList.Api.Constants.ValidationError;

namespace ToDoList.Api.Models.User
{
	public class UserChangePasswordModel
	{
		public string UserEmail { get; set; }
		public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}
