using Core.Models;

namespace Core.DTOs.ToDo
{
    /// <summary>
    /// Represents a simplified version of a To-Do item.
    /// </summary>
    public class ToDoDto
    {
        /// <summary>
        /// The description of the To-Do item.
        /// </summary>
        public string? Description { get; set; } = null;

        /// <summary>
        /// The difficulty level of the To-Do item.
        /// </summary>
        public Difficulty Difficulty { get; set; } = Difficulty.None;

        /// <summary>
        /// The name of the category associated with the To-Do item.
        /// </summary>
        public string? ToDoCategoryName { get; set; } = null;

        /// <summary>
        /// The optional deadline for the To-Do item.
        /// </summary>
        public DateTime? Deadline { get; set; } = null;

        /// <summary>
        /// The repeat frequency of the To-Do item.
        /// </summary>
        public RepeatFrequency RepeatFrequency { get; set; } = RepeatFrequency.None;
    }
}
