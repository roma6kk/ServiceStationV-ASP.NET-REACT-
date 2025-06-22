using System;
using System.Collections.Generic;

namespace ServiceStationV.DataAccess.Entities
{
    public class OrderEntity
    {
        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }
        public UserEntity Customer { get; set; }

        public string VehicleInfo { get; set; } = string.Empty;

        public ICollection<OrderServiceEntity> ServiceItems { get; set; } = new List<OrderServiceEntity>();

        public decimal TotalPrice { get; set; }

        public string Status { get; set; } = "Ожидает";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? PlannedDate { get; set; }
        public DateTime? CompletedAt { get; set; }

        public string? Comment { get; set; }
    }
}
