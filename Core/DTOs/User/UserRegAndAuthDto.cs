namespace Core.DTOs.User
{
    /// <summary>
    /// Represents the data required for user registration and authentication.
    /// </summary>
    public class UserRegAndAuthDto
    {
        /// <summary>
        /// The username of the user.
        /// </summary>
        public string? UserName { get; set; } = null;

        /// <summary>
        /// The password for user authentication.
        /// </summary>
        public string? Password { get; set; } = null;
    }
}
