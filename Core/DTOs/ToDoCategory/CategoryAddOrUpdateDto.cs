namespace Core.DTOs.ToDoCategory
{
    /// <summary>
    /// Represents the data required to add or update a To-Do category.
    /// </summary>
    public class CategoryAddOrUpdateDto
    {
        /// <summary>
        /// The unique identifier of the To-Do category.
        /// </summary>
        public Guid ToDoCategoryId { get; set; } = new Guid();

        /// <summary>
        /// The unique identifier of the user associated with this category.
        /// </summary>
        public Guid UserId { get; set; } = new Guid();

        /// <summary>
        /// The name of the To-Do category.
        /// </summary>
        public string? ToDoCategoryName { get; set; } = null;
    }
}
