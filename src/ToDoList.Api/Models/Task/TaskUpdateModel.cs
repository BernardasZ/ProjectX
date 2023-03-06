using DataModel.Enums;
using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.Task;

public class TaskUpdateModel : BaseValidatableObject
{
    public int Id { get; set; }

    public string TaskName { get; set; }

    public TaskStatusEnum Status { get; set; }

	protected override BaseValidator Validate() => new BaseValidator()
		.ValidateId<BaseValidator>(Id, nameof(Id))
		.ValidateString<BaseValidator>(TaskName, nameof(TaskName));
}