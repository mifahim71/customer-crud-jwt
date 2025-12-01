using System.Security.Claims;

namespace CustomerCrudApi.Services
{
    public interface IJwtService
    {
        string GenerateToken(IEnumerable<Claim>? claims);
    }
}
