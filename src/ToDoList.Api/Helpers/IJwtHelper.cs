namespace ToDoList.Api.Helpers;

public interface IJwtHelper
{
	string ConstructUserJwt(string role, string userId);
}