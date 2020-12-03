using Swashbuckle.AspNetCore.Annotations;

namespace ToDoList.Api.Models
{
    public class TaskDTO
	{
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TaskName { get; set; }
        public byte Status { get; set; }
    }
}
