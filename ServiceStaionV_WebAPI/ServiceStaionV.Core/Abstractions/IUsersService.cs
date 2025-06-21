
using ServiceStationV.Core.Models;

namespace ServiceStationV.Application.Services
{
    public interface IUsersService
    {
        Task<string> Login(string phone, string password);
        Task<User?> Register(string username, string email, string phone, string password);
    }
}