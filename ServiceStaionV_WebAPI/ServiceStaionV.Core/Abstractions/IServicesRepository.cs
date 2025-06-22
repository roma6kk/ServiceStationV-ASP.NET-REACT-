using ServiceStationV.Core.Models;

namespace ServiceStationV.Core.Abstractions
{
    public interface IServicesRepository
    {
        Task<List<Service>> Get();
        Task<Service> GetById(Guid id);
        Task<Guid> Create(Service service);
        Task<Guid> Update(Guid id, string name, string description, decimal price, string imagePath);
        Task<Guid> Delete(Guid id);
        Task<List<Service>> GetByIds(List<Guid> ids);
    }
}