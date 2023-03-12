using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.Entities.ProjectX;

public class BaseEntity
{
	[Key]
	[Column("id")]
	public int Id { get; set; }
}