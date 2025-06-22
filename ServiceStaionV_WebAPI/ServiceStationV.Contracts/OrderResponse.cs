
namespace ServiceStationV.Contracts
{
    public class OrderResponse
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public string VehicleInfo { get; set; } = string.Empty;
        public List<Guid> ServiceIds { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? PlannedDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? Comment { get; set; }

    }
}
