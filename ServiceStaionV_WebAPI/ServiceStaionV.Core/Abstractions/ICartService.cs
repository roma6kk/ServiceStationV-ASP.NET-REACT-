using ServiceStationV.Core.Models;

namespace ServiceStationV.Core.Abstractions
{
    public interface ICartService
    {
        Task Add(Guid serviceId, Guid userId);
        Task<List<Service>> Get(Guid userId);
        Task Remove(Guid serviceId, Guid userId);
        Task RemoveAll(Guid userId);
    }
}