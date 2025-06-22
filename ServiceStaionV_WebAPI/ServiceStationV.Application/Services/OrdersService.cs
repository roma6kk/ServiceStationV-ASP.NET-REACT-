using ServiceStationV.Contracts;
using ServiceStationV.Core.Abstractions;
using ServiceStationV.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
namespace ServiceStationV.Application.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IServicesRepository _servicesRepository;
        public OrdersService(IOrdersRepository ordersRepository, IServicesRepository servicesRepository)
        {
            _ordersRepository = ordersRepository;
            _servicesRepository = servicesRepository;
        }

        public async Task<List<Order>> GetAllOrders()
        {
            return await _ordersRepository.Get();
        }

        public async Task<Order?> GetOrderById(Guid id)
        {
            return await _ordersRepository.GetById(id);
        }

        public async Task<List<Order>> GetOrdersByUserId(Guid id)
        {
            return await _ordersRepository.GetByUserId(id);
        }

        public async Task<Guid> CreateOrder(Guid userId, OrderRequest orderRequest)    
        {
            var (order, error) = Order.Create(Guid.NewGuid(), userId, orderRequest.VehicleInfo, orderRequest.ServiceIds, orderRequest.TotalPrice, orderRequest.Status, DateTime.UtcNow, null, null, null, orderRequest.Comment);
            if (order == null)
            { 
                 throw new ValidationException(error);
            }
            return await _ordersRepository.Create(order);
        }


        public async Task<bool> UpdateOrder(
            Guid id,
            string vehicleInfo,
            List<Guid> serviceIds,
            decimal totalPrice,
            string status,
            DateTime? plannedDate,
            DateTime? completedAt,
            string? comment)
        {
            return await _ordersRepository.Update(id, vehicleInfo, serviceIds, totalPrice, status, plannedDate, completedAt, comment);
        }

        public async Task<bool> DeleteOrder(Guid id)
        {
            return await _ordersRepository.Delete(id);
        }
    }
}
