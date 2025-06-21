using Microsoft.EntityFrameworkCore;
using ServiceStationV.Core.Abstractions;
using ServiceStationV.Core.Models;
using ServiceStationV.DataAccess;
using ServiceStationV.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationV.Application.Services
{
    public class FavouriteService : IFavouriteService
    {
        private readonly IFavouriteServicesRepository _favouriteServicesRepository;
        public FavouriteService(IFavouriteServicesRepository favouriteServicesRepository) 
        {
            _favouriteServicesRepository = favouriteServicesRepository;
        }
        public async Task Add(Guid serviceId, Guid userId)
        {
            await _favouriteServicesRepository.AddService(serviceId, userId);
        }
        
        public async Task Remove(Guid serviceId, Guid userId)
        {
            await _favouriteServicesRepository.RemoveService(serviceId, userId);
        }

        public async Task RemoveAll(Guid userId)
        {
            await _favouriteServicesRepository.RemoveAllServices(userId);
        }

        public async Task<List<Service>> Get(Guid userId)
        {
            return await _favouriteServicesRepository.Get(userId);
        }
    }
}
