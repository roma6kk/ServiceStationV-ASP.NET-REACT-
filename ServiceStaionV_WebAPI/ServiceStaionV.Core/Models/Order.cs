using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationV.Core.Models
{
    public class Order
    {
        private Order(
            Guid id,
            Guid customerId,
            string vehicleInfo,
            List<Guid> serviceIds,
            decimal totalPrice,
            string status,
            DateTime createdAt,
            DateTime? updatedAt,
            DateTime? plannedDate,
            DateTime? completedAt,
            string? comment)
        {
            Id = id;
            CustomerId = customerId;
            VehicleInfo = vehicleInfo;
            ServiceIds = serviceIds;
            TotalPrice = totalPrice;
            Status = status;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            PlannedDate = plannedDate;
            CompletedAt = completedAt;
            Comment = comment;
        }

        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string VehicleInfo { get; set; } = string.Empty;
        public List<Guid> ServiceIds { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? PlannedDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? Comment { get; set; }

        public static (Order? Order, string Error) Create(
            Guid id,
            Guid customerId,
            string vehicleInfo,
            List<Guid> serviceIds,
            decimal totalPrice,
            string status,
            DateTime createdAt,
            DateTime? updatedAt,
            DateTime? plannedDate,
            DateTime? completedAt,
            string? comment)
        {
            var errors = new List<string>();

            if (customerId == Guid.Empty)
                errors.Add("Customer ID не может быть пустым.");

            if (string.IsNullOrWhiteSpace(vehicleInfo))
                errors.Add("Информация об автомобиле обязательна.");

            if (serviceIds == null || serviceIds.Count == 0)
                errors.Add("Необходимо указать хотя бы одну услугу.");

            if (totalPrice < 0)
                errors.Add("Общая сумма заказа не может быть отрицательной.");

            if (plannedDate.HasValue && plannedDate < createdAt)
                errors.Add("Планируемая дата не может быть раньше даты создания.");

            if (completedAt.HasValue && completedAt < createdAt)
                errors.Add("Дата завершения не может быть раньше даты создания.");

            if (comment?.Length > 300)
                errors.Add("Комментарий не должен превышать 300 символов.");

            if (errors.Count > 0)
                return (null, string.Join(" ", errors));

            var order = new Order(
                id,
                customerId,
                vehicleInfo,
                serviceIds,
                totalPrice,
                status ?? "Ожидает",
                createdAt,
                updatedAt,
                plannedDate,
                completedAt,
                comment);

            return (order, string.Empty);
        }
    }
}
