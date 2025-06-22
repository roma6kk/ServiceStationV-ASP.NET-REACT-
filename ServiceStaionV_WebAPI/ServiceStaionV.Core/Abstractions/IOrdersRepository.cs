using ServiceStationV.Core.Models;

namespace ServiceStationV.Core.Abstractions;

public interface IOrdersRepository
{
    Task<Guid> Create(Order order);
    Task<bool> Delete(Guid id);
    Task<List<Order>> Get();
    Task<Order?> GetById(Guid id);
    Task<bool> Update(Guid id, string vehicleInfo, List<Guid> serviceIds, decimal totalPrice, string status, DateTime? plannedDate, DateTime? completedAt, string? comment);
}