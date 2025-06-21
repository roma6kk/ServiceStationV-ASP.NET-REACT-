using ServiceStationV.Core.Models;

namespace ServiceStationV.Infrastructure
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}