using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Api.Models.Task;
using ToDoList.Api.Models.User;

namespace ToDoList.Api.Helpers
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<UserLoginModel, UserModel>();
            CreateMap<TaskModel, DataModel.Entities.ProjectX.Task>();
        }
    }
}
