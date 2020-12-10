using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.User
{
	public class UserUpdateModel : IValidatableObject
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            UserValidator validator = new UserValidator();

            validator.ValidateUserId(UserId);
            validator.ValidateEmail(UserEmail);

            return validator.GetValidationResults();
        }
    }
}
