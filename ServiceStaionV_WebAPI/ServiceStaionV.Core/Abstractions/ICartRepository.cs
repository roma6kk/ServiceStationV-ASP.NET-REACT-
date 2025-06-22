using ServiceStationV.Core.Models;

namespace ServiceStationV.Core.Abstractions
{
    public interface ICartRepository
    {
        Task AddService(Guid serviceId, Guid userId);
        Task<List<Service>> Get(Guid userId);
        Task RemoveAllServices(Guid userId);
        Task RemoveService(Guid serviceId, Guid userId);
    }
}