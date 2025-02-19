using Core.Interfaces;
using Core.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication
{
    /// <summary>
    /// Service for generating JSON Web Tokens (JWT) for user authentication.
    /// </summary>
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtTokenService"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration instance containing JWT settings.</param>
        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generates a JWT token for the given user.
        /// </summary>
        /// <param name="user">The user object containing the necessary information to create the token.</param>
        /// <returns>
        /// A task representing the asynchronous operation, with a string containing the generated JWT token.
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="user"/> is null.</exception>
        /// <remarks>
        /// The generated token includes:
        /// - <see cref="JwtRegisteredClaimNames.Sub"/> (subject) claim set to the user's username.
        /// - <see cref="JwtRegisteredClaimNames.Jti"/> (JWT ID) claim set to a new GUID.
        /// - The token is signed using HMAC SHA-256 with a secret key specified in the application configuration.
        /// - The token expires in 1 day from the time of creation.
        /// </remarks>
        public Task<string> GenerateTokenAsync(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var expiration = DateTime.UtcNow.Date.AddDays(1).AddTicks(-1);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
