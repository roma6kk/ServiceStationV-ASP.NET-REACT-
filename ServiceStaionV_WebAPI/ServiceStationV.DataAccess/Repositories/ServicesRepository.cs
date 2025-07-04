﻿using Microsoft.EntityFrameworkCore;
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
    public class ServicesRepository : IServicesRepository
    {
        private ServiceStationVDBContext _context;
        public ServicesRepository(ServiceStationVDBContext context)
        {
            _context = context;
        }
        public async Task<List<Service>> Get()
        {
            var serviceEntities = await _context.Services.AsNoTracking().ToListAsync();
            var services = serviceEntities.Select(s => Service.Create(id: s.Id,
                                                    name: s.Name,
                                                    description: s.Description,
                                                    price: s.Price,
                                                    imagePath: s.ImagePath).Service).ToList();
            return services;
        }

        public async Task<Service> GetById(Guid id)
        {
            var serviceEntity = await _context.Services.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            return Service.Create(id: serviceEntity.Id,
                name: serviceEntity.Name,
                description: serviceEntity.Description,
                price: serviceEntity.Price,
                imagePath: serviceEntity.ImagePath).Service;
        }

        public async Task<List<Service>> GetByIds(List<Guid> ids)
        {
            var serviceEntities = await _context.Services
                .AsNoTracking()
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();

            return serviceEntities
                .Select(e => Service.Create(e.Id, e.Name, e.Description, e.Price, e.ImagePath).Service)
                .ToList();
        }


        public async Task<Guid> Create(Service service)
        {
            var serviceEntity = new ServiceEntity
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description,
                Price = service.Price,
                ImagePath = service.ImagePath
            };

            await _context.AddAsync(serviceEntity);
            await _context.SaveChangesAsync();
            return serviceEntity.Id;
        }

        public async Task<Guid> Update(Guid id, string name, string description, decimal price, string imagePath)
        {
            await _context.Services.Where(s => s.Id == id).ExecuteUpdateAsync(s => s
            .SetProperty(s => s.Name, s => name)
            .SetProperty(s => s.Price, s => price)
            .SetProperty(s => s.ImagePath, s => imagePath)
            .SetProperty(s => s.Description, s => description));
            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.Services.Where(s => s.Id == id).ExecuteDeleteAsync();
            return id;  
        }


    }
}
