using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Entities.ProjectX
{
    [Table("user_session", Schema = "ProjectX")]
    public class UserSession
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("session_identifier")]
        [StringLength(255)]
        public string SessionIdentifier { get; set; }
        [Required]
        [Column("create_dt")]
        [StringLength(255)]
        public DateTime CreateDt { get; set; }
        [Required]
        [Column("ip")]
        [StringLength(15)]
        public string Ip { get; set; }
    }
}
