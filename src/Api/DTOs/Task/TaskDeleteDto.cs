using Domain.Validators;

namespace Api.DTOs.Task;

public class TaskDeleteDto : BaseValidatableObject
{
	public int Id { get; set; }
	public int UserId { get; set; }

	protected override IBaseValidator<ITaskValidator> Validate() => new TaskValidator()
		.ValidateId(Id, nameof(Id))
		.ValidateId(UserId, nameof(UserId));
}