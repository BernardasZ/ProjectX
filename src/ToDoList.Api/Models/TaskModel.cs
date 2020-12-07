using DataModel.Enums;

namespace ToDoList.Api.Models
{
    public class TaskModel
	{
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TaskName { get; set; }
        public TaskStatusEnum Status { get; set; }
    }
}
