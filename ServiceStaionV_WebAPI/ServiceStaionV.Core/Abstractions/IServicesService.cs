using ServiceStationV.Core.Models;

namespace ServiceStationV.Application.Services
{
    public interface IServicesService
    {
        Task<List<Service>> GetAllServices();
        Task<Guid> CreateService(Service service);
        Task<Guid> UpdateService(Guid id, string name, string description, decimal price, string imagePath);
        Task<Guid> DeleteService(Guid id);
    }
}