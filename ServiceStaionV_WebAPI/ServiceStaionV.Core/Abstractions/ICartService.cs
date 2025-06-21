using ServiceStationV.Core.Models;

namespace ServiceStationV.Application.Services
{
    public interface ICartService
    {
        Task Add(Guid serviceId, Guid userId);
        Task<List<Service>> Get(Guid userId);
        Task Remove(Guid serviceId, Guid userId);
        Task RemoveAll(Guid userId);
    }
}