using Microsoft.EntityFrameworkCore;
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

            bool exists = await _context.UserFavourites
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

            var favourite = new UserFavouriteEntity
            {
                ServiceId = serviceId,
                UserId = userId
            };

            await _context.UserFavourites.AddAsync(favourite);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveService(Guid serviceId, Guid userId)
        {
            var favourite = await _context.UserFavourites
                .FirstOrDefaultAsync(f => f.ServiceId == serviceId && f.UserId == userId);

            if (favourite != null)
            {
                _context.UserFavourites.Remove(favourite);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveAllServices(Guid userId)
        {
            var favourites = await _context.UserFavourites
                .Where(f => f.UserId == userId)
                .ToListAsync();

            _context.UserFavourites.RemoveRange(favourites);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Service>> Get(Guid userId)
        {
            return await _context.UserFavourites
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
