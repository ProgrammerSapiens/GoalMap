using Core.Models;

namespace Core.DTOs.ToDo
{
    /// <summary>
    /// Represents the data required to add a new To-Do item.
    /// </summary>
    public class ToDoAddDto
    {
        /// <summary>
        /// The description of the new To-Do item.
        /// </summary>
        public string? Description { get; set; } = null;

        /// <summary>
        /// The time block (day, week, month, year) for the To-Do item.
        /// </summary>
        public TimeBlock TimeBlock { get; set; } = TimeBlock.Day;

        /// <summary>
        /// The difficulty level of the To-Do item.
        /// </summary>
        public Difficulty Difficulty { get; set; } = Difficulty.None;

        /// <summary>
        /// The date when the To-Do item is scheduled.
        /// </summary>
        public DateTime ToDoDate { get; set; } = DateTime.MinValue;

        /// <summary>
        /// The name of the category associated with the To-Do item.
        /// </summary>
        public string? ToDoCategoryName { get; set; } = null;

        /// <summary>
        /// The unique identifier of the user who owns the To-Do item.
        /// </summary>
        public Guid UserId { get; set; } = new Guid();

        /// <summary>
        /// The optional deadline for the To-Do item.
        /// </summary>
        public DateTime? Deadline { get; set; } = null;

        /// <summary>
        /// The optional unique identifier of the parent To-Do item (for subtasks).
        /// </summary>
        public Guid? ParentToDoId { get; set; } = new Guid();

        /// <summary>
        /// The repeat frequency of the To-Do item.
        /// </summary>
        public RepeatFrequency RepeatFrequency { get; set; } = RepeatFrequency.None;
    }
}
