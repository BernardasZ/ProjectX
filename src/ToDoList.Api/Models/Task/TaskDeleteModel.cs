using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.Task;

public class TaskDeleteModel : BaseValidatableObject
{
	public int Id { get; set; }
	public int UserId { get; set; }

	protected override BaseValidator Validate() => new BaseValidator()
		.ValidateId<BaseValidator>(Id, nameof(Id))
		.ValidateId<BaseValidator>(UserId, nameof(UserId));
}
