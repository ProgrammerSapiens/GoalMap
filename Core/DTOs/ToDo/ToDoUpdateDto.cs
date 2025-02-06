using Core.Models;

namespace Core.DTOs.ToDo
{
    public class ToDoUpdateDto
    {
        public Guid ToDoId { get; set; } = Guid.NewGuid();
        public string? Description { get; set; } = null;
        public Difficulty Difficulty { get; set; } = Difficulty.None;
        public DateTime? Deadline { get; set; } = null;
        public DateTime ToDoDate { get; set; } = DateTime.MinValue;
        public bool CompletionStatus { get; set; } = false;
        public RepeatFrequency RepeatFrequency { get; set; } = RepeatFrequency.None;
        public string? ToDoCategoryName { get; set; } = null;
    }
}
