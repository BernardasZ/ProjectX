using Api.Validators;

namespace Api.Models.Task;

public class TaskDeleteModel : BaseValidatableObject, IBaseModel
{
	public int Id { get; set; }
	public int UserId { get; set; }

	protected override IBaseValidator<ITaskValidator> Validate() => new TaskValidator()
		.ValidateId(Id, nameof(Id))
		.ValidateId(UserId, nameof(UserId));
}