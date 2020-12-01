using DataModel.Enums;
using System.Collections.Generic;

namespace DataModel.Entities.ProjectX
{
    public class UserRole
    {
        public UserRole()
        {
            User = new HashSet<User>();
            UserAction = new HashSet<UserAction>();
        }

        public int Id { get; set; }
        public string RoleName { get; set; }
        public UserRoleEnum RoleValue { get; set; }

        public virtual ICollection<User> User { get; set; }
        public virtual ICollection<UserAction> UserAction { get; set; }
    }
}
