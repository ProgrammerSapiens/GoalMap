using Core.Models;

namespace Core.DTOs.ToDo
{
    /// <summary>
    /// Represents the data required to update a To-Do item.
    /// </summary>
    public class ToDoUpdateDto
    {
        /// <summary>
        /// The unique identifier of the To-Do item.
        /// </summary>
        public Guid ToDoId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The description of the To-Do item.
        /// </summary>
        public string? Description { get; set; } = null;

        /// <summary>
        /// The difficulty level of the To-Do item.
        /// </summary>
        public Difficulty Difficulty { get; set; } = Difficulty.None;

        /// <summary>
        /// The optional deadline for the To-Do item.
        /// </summary>
        public DateTime? Deadline { get; set; } = null;

        /// <summary>
        /// The date when the To-Do item is scheduled.
        /// </summary>
        public DateTime ToDoDate { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Indicates whether the To-Do item is completed.
        /// </summary>
        public bool CompletionStatus { get; set; } = false;

        /// <summary>
        /// The repeat frequency of the To-Do item.
        /// </summary>
        public RepeatFrequency RepeatFrequency { get; set; } = RepeatFrequency.None;

        /// <summary>
        /// The name of the category associated with the To-Do item.
        /// </summary>
        public Guid ToDoCategoryId { get; set; } = Guid.Empty;
    }
}
