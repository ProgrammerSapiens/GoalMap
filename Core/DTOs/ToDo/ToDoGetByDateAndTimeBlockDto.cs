using Core.Models;

namespace Core.DTOs.ToDo
{
    public class ToDoGetByDateAndTimeBlockDto
    {
        public DateTime Date { get; set; }
        public TimeBlock TimeBlock { get; set; }
    }
}
