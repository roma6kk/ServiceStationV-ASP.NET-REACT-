using ServiceStationV.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationV.Core.Abstractions
{
    public interface IFavouriteServicesRepository
    {
        Task AddService(Guid serviceId, Guid userId);
        Task RemoveAllServices(Guid userId);
        Task RemoveService(Guid serviceId, Guid userId);
        Task<List<Service>> Get(Guid userid);
}
}
