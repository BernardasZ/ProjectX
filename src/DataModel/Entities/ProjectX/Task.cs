using DataModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Entities.ProjectX
{
    [Table("task", Schema = "ProjectX")]
    public class Task
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("user_id")]
        public int UserId { get; set; }
        [Required]
        [Column("task_name")]
        [StringLength(1000)]
        public string TaskName { get; set; }
        [Column("task_status", TypeName = "tinyint unsigned")]
        public TaskStatusEnum Status { get; set; }
    }
}
