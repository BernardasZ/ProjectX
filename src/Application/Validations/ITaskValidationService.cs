using Domain.Models;

namespace Application.Validations;

public interface ITaskValidationService
{
	void CheckIfTaskNotNull(TaskModel model);
}