using Application.Enums;
using Application.Exceptions;
using Domain.Models;

namespace Application.Validations;

public class TaskValidationService : ITaskValidationService
{
	public void CheckIfTaskNotNull(TaskModel model)
	{
		if (model == null)
		{
			throw new ValidationException(ValidationErrorCodes.TaskDoesNotExist);
		}
	}
}