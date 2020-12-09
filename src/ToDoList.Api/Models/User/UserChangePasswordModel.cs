namespace ToDoList.Api.Models.User
{
	public class UserChangePasswordModel
	{
		public string UserEmail { get; set; }
		public string NewPassword { get; set; }
        public string OldPassword { get; set; }
    }
}
