using DataModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.Api.Validators;

namespace ToDoList.Api.Models.User
{
    public class UserCreateModel : IValidatableObject
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Password { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            UserValidator validator = new UserValidator();

            validator.ValidateName(UserName);
            validator.ValidateEmail(UserEmail);
            validator.ValidatePassword(Password);

            return validator.GetValidationResults();
        }
    }
}
