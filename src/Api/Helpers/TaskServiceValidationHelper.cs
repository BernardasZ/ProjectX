using Api.Exeptions;
using Persistence.Entities.ProjectX;

namespace Api.Helpers;

public class TaskServiceValidationHelper : ITaskServiceValidationHelper
{
	public void CheckIfNotNull(Task model)
	{
		if (model == null)
		{
			throw new GenericException(Enums.GenericError.TaskDoesNotExist);
		}
	}
}