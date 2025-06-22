using ServiceStationV.Core.Models;

namespace ServiceStationV.Core.Abstractions
{
    public interface IServicesService
    {
        Task<List<Service>> GetAllServices();
        Task<Service> GetServiceById(Guid id);
        Task<Guid> CreateService(Service service);
        Task<Guid> UpdateService(Guid id, string name, string description, decimal price, string imagePath);
        Task<Guid> DeleteService(Guid id);
        Task<List<Service>> GetServicesByIds(List<Guid> ids);
    }
}