using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Entities.ProjectX;

[Table("permission_controller", Schema = "ProjectX")]
public class PermissionController
{
    public PermissionController()
    {
        PermissionAction = new HashSet<PermissionAction>();
        PermissionMapping = new HashSet<PermissionMapping>();
    }

    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Required]
    [Column("controller_name")]
    [StringLength(255)]
    public string ControllerName { get; set; }

    [InverseProperty("Controller")]
    public virtual ICollection<PermissionAction> PermissionAction { get; set; }
    [InverseProperty("Controller")]
    public virtual ICollection<PermissionMapping> PermissionMapping { get; set; }
}
