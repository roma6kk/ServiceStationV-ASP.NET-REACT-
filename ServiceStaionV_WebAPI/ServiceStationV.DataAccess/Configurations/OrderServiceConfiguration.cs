using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceStationV.DataAccess.Entities;

namespace ServiceStationV.DataAccess.Configurations
{
    public class OrderServiceConfiguration : IEntityTypeConfiguration<OrderServiceEntity>
    {
        public void Configure(EntityTypeBuilder<OrderServiceEntity> builder)
        {
            builder.HasKey(os => new { os.OrderId, os.ServiceId });

            builder.HasOne(os => os.Order)
                .WithMany(o => o.ServiceItems)
                .HasForeignKey(os => os.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(os => os.Service)
                .WithMany(s => s.OrderItems)
                .HasForeignKey(os => os.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
