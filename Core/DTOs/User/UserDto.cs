namespace Core.DTOs.User
{
    public class UserDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public int Experience { get; set; }
        public int Level { get; set; }
    }
}
