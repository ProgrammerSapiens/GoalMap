using Core.Models;

namespace Core.DTOs.ToDo
{
    public class ToDoDto
    {
        public string? Description { get; set; } = null;
        public Difficulty Difficulty { get; set; } = Difficulty.None;
        public string? ToDoCategoryName { get; set; } = null;
        public DateTime? Deadline { get; set; } = null;
        public RepeatFrequency RepeatFrequency { get; set; } = RepeatFrequency.None;
    }
}
