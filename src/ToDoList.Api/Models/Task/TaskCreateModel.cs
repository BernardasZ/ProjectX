using DataModel.Enums;
using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.Task;

public class TaskCreateModel : BaseValidatableObject, IBaseModel
{
	public int UserId { get; set; }

	public string TaskName { get; set; }

	public TaskStatusEnum Status { get; set; }

	protected override IBaseValidator<ITaskValidator> Validate() => new TaskValidator()
		.ValidateId(UserId, nameof(UserId))
		.ValidateString(TaskName, nameof(TaskName));
}