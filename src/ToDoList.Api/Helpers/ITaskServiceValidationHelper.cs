using DataModel.Entities.ProjectX;

namespace ToDoList.Api.Helpers;

public interface ITaskServiceValidationHelper
{
	void ValidateTaskData(Task model);
}
