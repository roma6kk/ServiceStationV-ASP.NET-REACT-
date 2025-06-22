using ServiceStationV.Contracts;
using ServiceStationV.Core.Models;

namespace ServiceStationV.Core.Abstractions;

public interface IOrdersService
{
    Task<Guid> CreateOrder(Guid userId, OrderRequest orderRequest);
    Task<bool> DeleteOrder(Guid id);
    Task<List<Order>> GetAllOrders();
    Task<Order?> GetOrderById(Guid id);
    Task<bool> UpdateOrder(Guid id, string vehicleInfo, List<Guid> serviceIds, decimal totalPrice, string status, DateTime? plannedDate, DateTime? completedAt, string? comment);
}