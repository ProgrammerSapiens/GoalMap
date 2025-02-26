using Core.Interfaces;

namespace Authentication
{
    /// <summary>
    /// Service for hashing and verifying passwords using the BCrypt algorithm.
    /// </summary>
    public class PasswordHasher : IPasswordHasher
    {
        private readonly ILogger<PasswordHasher> _logger;

        public PasswordHasher(ILogger<PasswordHasher> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Asynchronously hashes a plain-text password.
        /// </summary>
        /// <param name="password">The plain-text password to be hashed.</param>
        /// <returns>A task representing the asynchronous operation, with a string containing the hashed password.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="password"/> is null or empty.</exception>
        /// <remarks>
        /// This method uses the BCrypt algorithm to securely hash the provided password.
        /// The hashed password can be stored for later verification.
        /// </remarks>
        public Task<string> HashPasswordAsync(string password)
        {
            _logger.LogInformation($"HashPasswordAsync(string {password})");

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return Task.FromResult(hashedPassword);
        }

        /// <summary>
        /// Asynchronously verifies if the provided plain-text password matches the given hashed password.
        /// </summary>
        /// <param name="password">The plain-text password to be verified.</param>
        /// <param name="hashedPassword">The stored hashed password to compare against.</param>
        /// <returns>A task representing the asynchronous operation, with a boolean indicating whether the password is valid.</returns>
        /// <exception cref="ArgumentNullException">Thrown when either <paramref name="password"/> or <paramref name="hashedPassword"/> is null or empty.</exception>
        /// <remarks>
        /// This method uses the BCrypt algorithm to compare the plain-text password with the hashed password.
        /// The password verification is done in a secure manner using the BCrypt comparison method.
        /// </remarks>
        public Task<bool> VerifyPasswordAsync(string password, string hashedPassword)
        {
            _logger.LogInformation($"VerifyPasswordAsync(string {password}, string {hashedPassword})");

            bool isVerified = BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            return Task.FromResult(isVerified);
        }
    }
}
