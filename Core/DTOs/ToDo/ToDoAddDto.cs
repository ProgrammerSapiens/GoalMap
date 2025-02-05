using Core.Models;

namespace Core.DTOs.ToDo
{
    public class ToDoAddDto
    {
        public string Description { get; set; }
        public TimeBlock TimeBlock { get; set; }
        public Difficulty Difficulty { get; set; }
        public DateTime ToDoDate { get; set; }
        public string ToDoCategoryName { get; set; }
        public Guid UserId { get; set; }
        public DateTime? Deadline { get; set; }
        public Guid? ParentToDoId { get; set; }
        public RepeatFrequency RepeatFrequency { get; set; }
    }
}
