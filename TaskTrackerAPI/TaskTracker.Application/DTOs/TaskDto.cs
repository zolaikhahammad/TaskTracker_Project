using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Application
{
    public class TaskDto
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string PriorityName { get; set; } = string.Empty;
        public long TaskPriorityId { get; set; }
    }

}
