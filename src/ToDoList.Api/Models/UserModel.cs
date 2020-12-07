using DataModel.Enums;
using ExpressiveAnnotations.Attributes;
using System.ComponentModel.DataAnnotations;
using ToDoList.Api.Enums;
using static ToDoList.Api.Constants.ValidationError;

namespace ToDoList.Api.Models
{
    public class UserModel
    {
        public int UserId { get; set; }

        public UserRoleEnum Role { get; set; }

        [RequiredIf("UserEmail==null", ErrorMessage = UserCredentialRequired)]
        public string UserName { get; set; }

        [RequiredIf("UserName==null", ErrorMessage = UserCredentialRequired)]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = PasswordRequired)]
        [MinLength(12, ErrorMessage = PasswordLength)]
        public string Password { get; set; }
    }
}
