namespace Core.DTOs.ToDoCategory
{
    public class CategoryDto
    {
        public Guid ToDoCategoryId { get; set; } = new Guid();
        public string? ToDoCategoryName { get; set; } = null;
    }
}
