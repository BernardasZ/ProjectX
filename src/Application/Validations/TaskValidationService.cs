using Application.Exceptions;
using Application.Exceptions.Enums;
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