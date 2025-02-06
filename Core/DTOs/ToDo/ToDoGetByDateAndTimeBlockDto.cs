using Core.Models;

namespace Core.DTOs.ToDo
{
    /// <summary>
    /// Represents a request to retrieve To-Do items by date and time block.
    /// </summary>
    public class ToDoGetByDateAndTimeBlockDto
    {
        /// <summary>
        /// The date for which To-Do items should be retrieved.
        /// </summary>
        public DateTime Date { get; set; } = DateTime.MinValue;

        /// <summary>
        /// The time block (day, week, month, year) for filtering To-Do items.
        /// </summary>
        public TimeBlock TimeBlock { get; set; } = TimeBlock.Day;
    }
}
