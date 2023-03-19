using Api.Validators;
using Persistence.Enums;

namespace Api.Models.Task;

public class TaskUpdateModel : BaseValidatableObject, IBaseModel
{
	public int Id { get; set; }

	public int UserId { get; set; }

	public string Name { get; set; }

	public TaskStatus Status { get; set; }

	protected override IBaseValidator<ITaskValidator> Validate() => new TaskValidator()
		.ValidateId(Id, nameof(Id))
		.ValidateId(UserId, nameof(UserId))
		.ValidateString(Name, nameof(Name));
}