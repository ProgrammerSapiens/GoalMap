using Core.Models;

namespace Core.Interfaces
{
    /// <summary>
    /// Interface for services that handle JWT token generation.
    /// </summary>
    public interface IJwtTokenService
    {
        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="user">The user for whom the JWT token will be generated.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the generated JWT token as a string.</returns>
        Task<string> GenerateTokenAsync(User user);
    }
}
