using DataModel.Entities.ProjectX;
using ToDoList.Api.Exeptions;

namespace ToDoList.Api.Helpers;

public class TaskServiceValidationHelper : ITaskServiceValidationHelper
{
	public void ValidateTaskData(Task model)
	{
		if (model == null)
		{
			throw new GenericException(Enums.GenericError.TaskDoesNotExist);
		}
	}
}
