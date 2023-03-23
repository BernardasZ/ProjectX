using Domain.Enums;
using Domain.Exeptions;
using Domain.Models;

namespace Application.Validations;

public class TaskValidationService : ITaskValidationService
{
	public void CheckIfTaskNotNull(TaskModel model)
	{
		if (model == null)
		{
			throw new GenericException(GenericError.TaskDoesNotExist);
		}
	}
}