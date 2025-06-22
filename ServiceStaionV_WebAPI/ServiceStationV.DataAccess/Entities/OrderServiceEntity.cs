using System;

namespace ServiceStationV.DataAccess.Entities
{
    public class OrderServiceEntity
    {
        public Guid OrderId { get; set; }
        public OrderEntity Order { get; set; }

        public Guid ServiceId { get; set; }
        public ServiceEntity Service { get; set; }
    }
}
