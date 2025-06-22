using Microsoft.EntityFrameworkCore;
using ServiceStationV.Core.Abstractions;
using ServiceStationV.Core.Models;
using ServiceStationV.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationV.DataAccess.Repositories
{
    public class CartsRepository : ICartRepository
    {
        private ServiceStationVDBContext _context;
        public CartsRepository(ServiceStationVDBContext context)
        {
            _context = context;
        }
        public async Task AddService(Guid serviceId, Guid userId)
        {
            Console.WriteLine($"Получен serviceId: {serviceId}");

            bool exists = await _context.Carts
                .AsNoTracking()
                .AnyAsync(uf => uf.ServiceId == serviceId && uf.UserId == userId);
            Console.WriteLine($"Существует в базе? {exists}");

            var serviceExists = await _context.Services
    .AsNoTracking()
    .AnyAsync(s => s.Id == serviceId);


            if (!serviceExists)
            {
                throw new InvalidOperationException("Такой услуги не существует");
            }

            if (exists)
            {
                throw new InvalidOperationException("Эта услуга уже в корзине");
            }

            var cart = new CartEntity
            {
                ServiceId = serviceId,
                UserId = userId
            };

            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveService(Guid serviceId, Guid userId)
        {
            var cart = await _context.Carts  
                .FirstOrDefaultAsync(f => f.ServiceId == serviceId && f.UserId == userId);

            if (cart != null)
            {
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveAllServices(Guid userId)
        {
            var carts = await _context.Carts
                .Where(f => f.UserId == userId)
                .ToListAsync();

            _context.Carts.RemoveRange(carts);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Service>> Get(Guid userId)
        {
            return await _context.Carts
                .Where(f => f.UserId == userId)
                .Select(f => new Service(
                    f.Service.Id,
                    f.Service.Name,
                    f.Service.Description,
                    f.Service.Price,
                    f.Service.ImagePath
                ))
                .ToListAsync();
        }
    }
}
