using Core.Models;

namespace Core.DTOs.ToDo
{
    public class ToDoDto
    {
        public string Description { get; set; }
        public Difficulty Difficulty { get; set; }
        public string ToDoCategoryName { get; set; }
        public DateTime? Deadline { get; set; }
        public RepeatFrequency RepeatFrequency { get; set; }
    }
}
