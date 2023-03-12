using DataModel.Enums;
using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.Task;

public class TaskModel : BaseValidatableObject, IBaseModel
{
	public int Id { get; set; }

	public int UserId { get; set; }

	public string TaskName { get; set; }

	public TaskStatusEnum Status { get; set; }

	protected override IBaseValidator<ITaskValidator> Validate() => new TaskValidator()
		.ValidateId(Id, nameof(Id))
		.ValidateId(UserId, nameof(UserId))
		.ValidateString(TaskName, nameof(TaskName));
}