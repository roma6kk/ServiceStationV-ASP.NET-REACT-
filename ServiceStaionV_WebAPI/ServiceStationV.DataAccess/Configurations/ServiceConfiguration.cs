using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceStationV.DataAccess.Entities;

namespace ServiceStationV.DataAccess.Configurations
{
    public class ServiceConfiguration : IEntityTypeConfiguration<ServiceEntity>
    {
        public void Configure(EntityTypeBuilder<ServiceEntity> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(s => s.Price)
                .IsRequired();

            builder.Property(s => s.ImagePath)
                .IsRequired()
                .HasMaxLength(500);
            builder.HasMany(s => s.FavouritedByUsers)
    .WithOne(ufe => ufe.Service)
    .OnDelete(DeleteBehavior.ClientCascade);
        }
    }
}