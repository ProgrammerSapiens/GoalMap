using Core.Models;

namespace Core.DTOs.User
{
    public class UserUpdateDto
    {
        public string? UserName { get; set; } = string.Empty;
        public Difficulty Difficulty { get; set; } = Difficulty.None;
    }
}
