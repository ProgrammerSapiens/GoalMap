namespace Core.DTOs.ToDoCategory
{
    /// <summary>
    /// Represents the data required to add or update a To-Do category.
    /// </summary>
    public class CategoryAddDto
    {
        /// <summary>
        /// The name of the To-Do category.
        /// </summary>
        public string? ToDoCategoryName { get; set; } = null;
    }
}
