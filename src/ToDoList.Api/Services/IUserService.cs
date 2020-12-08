using DataModel.Entities.ProjectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Api.Models.User;

namespace ToDoList.Api.Services
{
	public interface IUserService
	{
		UserLoginResponseModel Login(UserLoginModel model);
		void Logout();
		void ChangePassword(UserChangePasswordModel model);
		void ResetPassword(UserResetPasswordModel model);
		void InitUserPasswordReset(UserModel model);
		bool CheckIfUserExists(UserModel model);
	}
}
