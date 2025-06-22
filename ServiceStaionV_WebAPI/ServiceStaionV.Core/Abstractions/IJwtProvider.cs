using ServiceStationV.Core.Models;

namespace ServiceStationV.Core.Abstractions
{
    public interface IJwtProvider
    {
        string GenerateToken(User user);
    }
}