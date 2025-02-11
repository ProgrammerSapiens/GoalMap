using Moq;
using Authentication;
using Core.Models;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace Tests.UnitTests.ServicesTests
{
    public class JwtTokenServiceTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly JwtTokenService _jwtTokenService;

        public JwtTokenServiceTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.SetupGet(c => c["Jwt:Key"]).Returns("Its-256-bit-long-secret-test-key-here");
            _configurationMock.SetupGet(c => c["Jwt:Issuer"]).Returns("mocked-issuer");
            _configurationMock.SetupGet(c => c["Jwt:Audience"]).Returns("mocked-audience");

            _jwtTokenService = new JwtTokenService(_configurationMock.Object);
        }

        [Fact]
        public async Task GenerateTokenAsync_WithValidUser_ReturnsValidJwtToken()
        {
            var user = new User("testUser");

            var token = await _jwtTokenService.GenerateTokenAsync(user);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            Assert.Equal("testUser", jwtToken.Subject);
            Assert.NotNull(jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti));
            Assert.Equal("mocked-issuer", jwtToken.Issuer);
            Assert.Equal("mocked-audience", jwtToken.Audiences.FirstOrDefault());
            Assert.True(jwtToken.ValidTo > DateTime.UtcNow);
        }

        [Fact]
        public async Task GenerateTokenAsync_WithInvalidKey_ThrowsException()
        {
            _configurationMock.SetupGet(c => c["Jwt:Key"]).Returns(string.Empty);
            var user = new User("testUser");

            await Assert.ThrowsAsync<ArgumentException>(() => _jwtTokenService.GenerateTokenAsync(user));
        }
    }
}
