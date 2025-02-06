namespace Core.DTOs.ToDoCategory
{
    /// <summary>
    /// Represents a To-Do category with basic information.
    /// </summary>
    public class CategoryDto
    {
        /// <summary>
        /// The unique identifier of the To-Do category.
        /// </summary>
        public Guid ToDoCategoryId { get; set; } = new Guid();

        /// <summary>
        /// The name of the To-Do category.
        /// </summary>
        public string? ToDoCategoryName { get; set; } = null;
    }
}
