using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Entities.ProjectX
{
    [Table("user_data", Schema = "ProjectX")]
    public class UserData
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("role_id")]
        public int RoleId { get; set; }
        [Column("user_name")]
        [StringLength(255)]
        public string UserName { get; set; }
        [Column("user_email")]
        [StringLength(255)]
        public string UserEmail { get; set; }
        [Required]
        [Column("pass_hash")]
        [StringLength(255)]
        public string PassHash { get; set; }

        [ForeignKey(nameof(RoleId))]
        [InverseProperty("UserData")]
        public virtual Role Role { get; set; }
    }
}
