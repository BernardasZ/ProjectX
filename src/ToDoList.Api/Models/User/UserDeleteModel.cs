using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.User
{
	public class UserDeleteModel : IValidatableObject
	{
		public int UserId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            UserValidator validator = new UserValidator();

            validator.ValidateUserId(UserId);

            return validator.GetValidationResults();
        }
    }
}
