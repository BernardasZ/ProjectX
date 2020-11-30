using System;
using System.Collections.Generic;

namespace DataModel.Entities.ProjectX
{
    public class User
    {
        public User()
        {
            ToDoTask = new HashSet<ToDoTask>();
        }

        public int Id { get; set; }
        public int RoleId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public virtual UserRole Role { get; set; }
        public virtual ICollection<ToDoTask> ToDoTask { get; set; }
    }
}
