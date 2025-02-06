namespace Core.DTOs.User
{
    /// <summary>
    /// Represents user-related data for display or processing.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// The unique identifier of the user.
        /// </summary>
        public Guid UserId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// The username of the user.
        /// </summary>
        public string? UserName { get; set; } = null;

        /// <summary>
        /// The user's accumulated experience points.
        /// </summary>
        public int Experience { get; set; } = 0;

        /// <summary>
        /// The user's current level based on experience.
        /// </summary>
        public int Level { get; set; } = 0;
    }
}
