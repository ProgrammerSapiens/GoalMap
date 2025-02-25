using Core.Models;

namespace Core.DTOs.ToDo
{
    /// <summary>
    /// Represents a simplified version of a To-Do item.
    /// </summary>
    public class ToDoDto
    {
        /// <summary>
        /// The unique identifier of the todo item.
        /// </summary>
        public Guid ToDoId { get; set; }

        /// <summary>
        /// The description of the To-Do item.
        /// </summary>
        public string? Description { get; set; } = null;

        /// <summary>
        /// The difficulty level of the To-Do item.
        /// </summary>
        public Difficulty Difficulty { get; set; } = Difficulty.None;

        /// <summary>
        /// The date of the Todo item.
        /// </summary>
        public DateTime ToDoDate { get; set; }

        /// <summary>
        /// The id of the category associated with the ToDo item.
        /// </summary>
        public Guid ToDoCategoryId { get; set; }

        /// <summary>
        /// The id of the user associated with the ToDo item.
        /// </summary>
        public Guid UserId { get; set; }

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
