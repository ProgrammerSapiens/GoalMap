namespace Core.Interfaces
{
    /// <summary>
    /// Provides methods for hashing passwords and verifying hashed passwords.
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// Hashes a plain text password and returns the hashed result.
        /// </summary>
        /// <param name="password">The plain text password.</param>
        /// <returns>The hashed password.</returns>
        Task<string> HashPasswordAsync(string password);

        /// <summary>
        /// Verifies if the provided password matches the stored hashed password.
        /// </summary>
        /// <param name="password">The plain text password.</param>
        /// <param name="hashedPassword">The stored hashed password.</param>
        /// <returns>
        /// <c>true</c> if the password matches; otherwise, <c>false</c>
        /// </returns>
        Task<bool> VerifyPasswordAsync(string password, string hashedPassword);
    }
}
