using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceStationV.Core.Models;
using ServiceStationV.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ServiceStationV.DataAccess.Configurations
{
    internal class ServiceConfiguration : IEntityTypeConfiguration<ServiceEntity>
    {
        public void Configure(EntityTypeBuilder<ServiceEntity> builder)
        {
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Name)
                .HasMaxLength(Service.MAX_NAME_LENGTH)
                .IsRequired();
            builder.Property(s => s.Description)
                .HasMaxLength(Service.MAX_DESCRIPTION_LENGTH)
                .IsRequired();
            builder.Property(s => s.Price)
                .IsRequired();
            builder.Property(s => s.ImagePath)
                .HasMaxLength(Service.MAX_IMAGEPATH_LENGTH)
                .IsRequired();
        }
    }
}
