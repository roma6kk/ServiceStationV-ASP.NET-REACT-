using ServiceStationV.Core.Abstractions;
using ServiceStationV.Core.Models;
using ServiceStationV.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationV.Application.Services
{
    public class ServicesService : IServicesService
    {
        private readonly IServicesRepository _servicesRepository;
        public ServicesService(IServicesRepository servicesRepository)
        {
            _servicesRepository = servicesRepository;
        }

        public async Task<List<Service>> GetAllServices()
        {
            return await _servicesRepository.Get();
        }
        public async Task<Service> GetServiceById(Guid id)
        {
            return await _servicesRepository.GetById(id);
        }
        public async Task<List<Service>> GetServicesByIds(List<Guid> ids)
        {
            return await _servicesRepository.GetByIds(ids);
        }
        public async Task<Guid> CreateService(Service service)
        {
            return await _servicesRepository.Create(service);
        }

        public async Task<Guid> UpdateService(Guid id, string name, string description, decimal price, string imagePath)
        {
            return await _servicesRepository.Update(id, name, description, price, imagePath);

        }

        public async Task<Guid> DeleteService(Guid id)
        {
            return await _servicesRepository.Delete(id);
        }
    }
}
