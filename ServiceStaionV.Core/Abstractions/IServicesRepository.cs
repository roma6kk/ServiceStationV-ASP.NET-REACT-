using ServiceStationV.Core.Models;

namespace ServiceStationV.DataAccess.Repositories
{
    public interface IServicesRepository
    {
        Task<List<Service>> Get();
        Task<Guid> Create(Service service);
        Task<Guid> Update(Guid id, string name, string description, decimal price, string imagePath);
        Task<Guid> Delete(Guid id);
    }
}