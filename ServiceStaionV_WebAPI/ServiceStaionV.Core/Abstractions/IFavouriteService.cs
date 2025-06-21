using ServiceStationV.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationV.Core.Abstractions
{
    public interface IFavouriteService
    {
        Task Add(Guid serviceId, Guid userId);
        Task Remove(Guid serviceId, Guid userId);
        Task RemoveAll(Guid userId);
        Task<List<Service>> Get(Guid userId);

    }
}
