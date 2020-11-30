using System;
using System.Collections.Generic;

namespace DataModel.Entities.ProjectX
{
    public class ToDoTask
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TaskName { get; set; }
        public byte Status { get; set; }

        public virtual User User { get; set; }
    }
}
