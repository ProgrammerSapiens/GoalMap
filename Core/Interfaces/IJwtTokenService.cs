using Core.Models;

namespace Core.Interfaces
{
    public interface IJwtTokenService
    {
        Task<string> GenerateTokenAsync(User user);
    }
}
