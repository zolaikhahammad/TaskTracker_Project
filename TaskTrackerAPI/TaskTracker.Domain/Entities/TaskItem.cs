namespace TaskTracker.Domain.Entities
{
    public class TaskItem
    {
        public DateTime CreatedAt { get; set; }
        public int CreatedBy { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; } = DateTime.Now;
        public bool IsCompleted { get; set; }
        public int TaskId { get; set; }
        public string Title { get; set; } = string.Empty;

        // Foreign Key
        public long TaskPriorityId { get; set; }
        public TaskPriorityItem? TaskPriority { get; set; }
        public string Category { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; }
        public int UpdatedBy { get; set; }

    }
}
