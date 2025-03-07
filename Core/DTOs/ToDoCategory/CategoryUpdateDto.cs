namespace Core.DTOs.ToDoCategory
{
    public class CategoryUpdateDto
    {
        /// <summary>
        /// The name of the To-Do category.
        /// </summary>
        public string? ToDoCategoryName { get; set; } = null;

        public Guid ToDoCategoryId { get; set; } = new Guid();
    }
}
