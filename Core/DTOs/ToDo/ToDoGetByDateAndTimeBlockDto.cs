using Core.Models;

namespace Core.DTOs.ToDo
{
    public class ToDoGetByDateAndTimeBlockDto
    {
        public DateTime Date { get; set; } = DateTime.MinValue;
        public TimeBlock TimeBlock { get; set; } = TimeBlock.Day;
    }
}
