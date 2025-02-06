namespace Core.DTOs.ToDoCategory
{
    public class CategoryAddOrUpdateDto
    {
        public Guid ToDoCategoryId { get; set; } = new Guid();
        public Guid UserId { get; set; } = new Guid();
        public string? ToDoCategoryName { get; set; } = null;
    }
}
