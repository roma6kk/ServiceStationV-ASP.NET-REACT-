namespace ServiceStationV.Contracts
{
    public class OrderRequest
    {
        public string VehicleInfo { get; set; } = string.Empty;
        public List<Guid> ServiceIds { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public string? Status { get; set; }
        public DateTime? PlannedDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string? Comment { get; set; }
    }
}
