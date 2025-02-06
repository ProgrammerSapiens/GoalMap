using Core.Models;

namespace Core.DTOs.ToDo
{
    public class ToDoAddDto
    {
        public string? Description { get; set; } = null;
        public TimeBlock TimeBlock { get; set; } = TimeBlock.Day;
        public Difficulty Difficulty { get; set; } = Difficulty.None;
        public DateTime ToDoDate { get; set; } = DateTime.MinValue;
        public string? ToDoCategoryName { get; set; } = null;
        public Guid UserId { get; set; } = new Guid();
        public DateTime? Deadline { get; set; } = null;
        public Guid? ParentToDoId { get; set; } = new Guid();
        public RepeatFrequency RepeatFrequency { get; set; } = RepeatFrequency.None;
    }
}
