using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Entities.ProjectX;

public class PermissionView
{
    [Required]
    [Column("role_name")]
    [StringLength(50)]
    public string RoleName { get; set; }
    [Required]
    [Column("controller_name")]
    [StringLength(255)]
    public string ControllerName { get; set; }
    [Required]
    [Column("action_name")]
    [StringLength(255)]
    public string ActionName { get; set; }
    [Column("allow_all_actions", TypeName = "bit(1)")]
    public bool AllowAllActions { get; set; }
}
