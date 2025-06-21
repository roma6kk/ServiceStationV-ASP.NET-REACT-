using ServiceStationV.Core.Models;
using ServiceStationV.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationV.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public async Task Add(Guid serviceId, Guid userId)
        {
            await _cartRepository.AddService(serviceId, userId);
        }

        public async Task Remove(Guid serviceId, Guid userId)
        {
            await _cartRepository.RemoveService(serviceId, userId);
        }

        public async Task RemoveAll(Guid userId)
        {
            await _cartRepository.RemoveAllServices(userId);
        }

        public async Task<List<Service>> Get(Guid userId)
        {
            return await _cartRepository.Get(userId);
        }
    }
}
