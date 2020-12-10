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
		IEnumerable<UserModel> GetUserList();
		void CreateUser(UserModel model);
		UserModel ReadUser(UserModel model);
		UserModel UpdateUser(UserModel model);
		void DeleteUser(UserModel model);
	}
}
