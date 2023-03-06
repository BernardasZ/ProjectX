using DataModel.Enums;
using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.Task;

public class TaskModel : BaseValidatableObject
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string TaskName { get; set; }

    public TaskStatusEnum Status { get; set; }

	protected override BaseValidator Validate() => new BaseValidator()
		.ValidateId<BaseValidator>(Id, nameof(Id))
		.ValidateId<BaseValidator>(UserId, nameof(UserId))
		.ValidateString<BaseValidator>(TaskName, nameof(TaskName));
}
