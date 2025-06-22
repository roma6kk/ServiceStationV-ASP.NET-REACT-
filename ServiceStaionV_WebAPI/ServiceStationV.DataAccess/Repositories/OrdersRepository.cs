using Microsoft.EntityFrameworkCore;
using ServiceStationV.Core.Abstractions;
using ServiceStationV.Core.Models;
using ServiceStationV.DataAccess.Entities;

namespace ServiceStationV.DataAccess.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly IServicesRepository _servicesRepository;
        private readonly ServiceStationVDBContext _context;

        public OrdersRepository(ServiceStationVDBContext context, IServicesRepository servicesRepository)
        {
            _context = context;
            _servicesRepository = servicesRepository;
        }

        public async Task<List<Order>> Get()
        {
            var orderEntities = await _context.Orders
                .Include(o => o.ServiceItems)
                    .ThenInclude(os => os.Service)
                .AsNoTracking()
                .ToListAsync();

            return orderEntities.Select(ToModel).ToList();
        }

        public async Task<Order?> GetById(Guid id)
        {
            var orderEntity = await _context.Orders
                .Include(o => o.ServiceItems)
                    .ThenInclude(os => os.Service)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);

            return orderEntity == null ? null : ToModel(orderEntity);
        }

        public async Task<List<Order>> GetByUserId(Guid id)
        {
            var orderEntities = await _context.Orders
                .AsNoTracking()
                .Where(o => o.CustomerId == id)
                .Include(o => o.ServiceItems)
                    .ThenInclude(os => os.Service)
                .ToListAsync();

            var orders = new List<Order>();

            foreach (var entity in orderEntities)
            {
                var serviceIds = entity.ServiceItems?
                    .Where(os => os.Service != null)
                    .Select(os => os.Service!.Id)
                    .ToList() ?? new List<Guid>();


                var (order, error) = Order.Create(
                    entity.Id,
                    entity.CustomerId,
                    entity.VehicleInfo,
                    serviceIds,
                    entity.TotalPrice,
                    entity.Status,
                    entity.CreatedAt,
                    entity.UpdatedAt,
                    entity.PlannedDate,
                    entity.CompletedAt,
                    entity.Comment);

                if (order != null)
                {
                    orders.Add(order);
                }
                else
                {
                    throw new Exception("Orders not found");
                }
            }

            return orders;
        }

        public async Task<Guid> Create(Order order)
        {
            var entity = ToEntity(order);
            await _context.Orders.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> Update(Guid id, string vehicleInfo, List<Guid> serviceIds, decimal totalPrice, string status, DateTime? plannedDate, DateTime? completedAt, string? comment)
        {
            var existing = await _context.Orders
                .Include(o => o.ServiceItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (existing == null)
                return false;

            existing.VehicleInfo = vehicleInfo;
            existing.TotalPrice = totalPrice;
            existing.Status = status;
            existing.PlannedDate = plannedDate;
            existing.CompletedAt = completedAt;
            existing.Comment = comment;
            existing.UpdatedAt = DateTime.UtcNow;

            // Обновим услуги: удалим старые и добавим новые
            _context.OrderServices.RemoveRange(existing.ServiceItems);
            var serviceItems = await _servicesRepository.GetByIds(serviceIds);
            existing.ServiceItems = serviceItems.Select(s => new OrderServiceEntity
            {
                OrderId = id,
                ServiceId = s.Id
            }).ToList();

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Guid id)
        {
            var existing = await _context.Orders.FindAsync(id);
            if (existing == null)
                return false;

            _context.Orders.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        // 🔁 Mapping helpers
        private static Order ToModel(OrderEntity entity)
        {
            var services = entity.ServiceItems.Select(os =>
                Service.Create(
                    os.Service.Id,
                    os.Service.Name,
                    os.Service.Description,
                    os.Service.Price,
                    os.Service.ImagePath
                ).Service).ToList();
            var Serviceids = services.Select(services => services.Id).ToList();
            return Order.Create(
                entity.Id,
                entity.CustomerId,
                entity.VehicleInfo,
                Serviceids,
                entity.TotalPrice,
                entity.Status,
                entity.CreatedAt,
                entity.UpdatedAt,
                entity.PlannedDate,
                entity.CompletedAt,
                entity.Comment
            ).Order;
        }

        private static OrderEntity ToEntity(Order order)
        {
            return new OrderEntity
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                VehicleInfo = order.VehicleInfo,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt,
                PlannedDate = order.PlannedDate,
                CompletedAt = order.CompletedAt,
                Comment = order.Comment,
                ServiceItems = order.ServiceIds.Select(serviceId => new OrderServiceEntity
                {
                    OrderId = order.Id,
                    ServiceId = serviceId
                }).ToList()
            };
        } 
    }
}
