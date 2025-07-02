using System.ComponentModel.DataAnnotations;

namespace TaskTracker.Application
{
    public class TaskUpdateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title can't be longer than 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description can't exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Priority is required")]
        public int PriorityId { get; set; } 

        [Required(ErrorMessage = "Due date is required")]
        public DateTime DueDate { get; set; }

        public bool IsCompleted { get; set; }
    }
}
