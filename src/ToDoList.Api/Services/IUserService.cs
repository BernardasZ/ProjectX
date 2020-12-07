using DataModel.Entities.ProjectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Api.Models;

namespace ToDoList.Api.Services
{
	public interface IUserService
	{
		UserModel Login(UserModel user);
		void LoginOut(UserModel user);
		void ChangePassword(UserPassChangeModel user);
		bool CheckIfUserExists(UserModel user);
		IQueryable<UserData> GetUserData(UserModel user);
		string GetNewJwt(UserModel user);
		bool CheckUserIdentity(string id);
	}
}
