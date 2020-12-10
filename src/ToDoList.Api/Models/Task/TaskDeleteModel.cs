using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.Task
{
	public class TaskDeleteModel : IValidatableObject
	{
		public int Id { get; set; }
		public int UserId { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			TaskValidator validator = new TaskValidator();

			validator.ValidateTaskId(Id);
			validator.ValidateUserId(UserId);

			return validator.GetValidationResults();
		}
	}
}
