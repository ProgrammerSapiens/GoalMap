using Core.Models;

namespace Core.DTOs.User
{
    /// <summary>
    /// Represents the data required to update a user's profile.
    /// </summary>
    public class UserUpdateDto
    {
        /// <summary>
        /// The updated username of the user.
        /// </summary>
        public string? UserName { get; set; } = string.Empty;
    }
}
