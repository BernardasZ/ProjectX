using DataModel.Enums;

namespace ToDoList.Api.Models.User
{
    public class UserModel
    {
        public int UserId { get; set; }
        public UserRoleEnum Role { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }
    }
}
