using Core.Models;

namespace Core.DTOs.ToDo
{
    public class ToDoUpdateDto
    {
        public Guid ToDoId { get; set; }
        public string Description { get; set; }
        public Difficulty Difficulty { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime ToDoDate { get; set; }
        public bool CompletionStatus { get; set; }
        public RepeatFrequency RepeatFrequency { get; set; }
        public string ToDoCategoryName { get; set; }
    }
}
