namespace Core.DTOs.ToDoCategory
{
    public class CategoryAddOrUpdateDto
    {
        public Guid ToDoCategoryId { get; set; }
        public Guid UserId { get; set; }
        public string ToDoCategoryName { get; set; }
    }
}
