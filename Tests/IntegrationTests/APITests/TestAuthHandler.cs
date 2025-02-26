using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Tests.IntegrationTests.APITests
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TestAuthHandler(
            IHttpContextAccessor httpContextAccessor,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            var userIdFromHeader = request?.Headers["X-Test-UserId"].FirstOrDefault();

            Guid userId = Guid.TryParse(userIdFromHeader, out var parsedUserId)
                ? parsedUserId
                : Guid.Parse("80a87a51-d544-4653-ae91-c6395e5fd8ce");

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, userId.ToString()),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };

            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
