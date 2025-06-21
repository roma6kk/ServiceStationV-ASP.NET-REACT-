using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ServiceStationV.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationV.DataAccess.Configurations
{
    public class CartConfiguration : IEntityTypeConfiguration<CartEntity>
    {
        public void Configure(EntityTypeBuilder<CartEntity> builder)
        {
            builder.HasKey(ce => new { ce.UserId, ce.ServiceId });

            builder.HasOne(ce => ce.User)
                .WithMany(u => u.Cart)
                .HasForeignKey(ce => ce.UserId)
                .OnDelete(DeleteBehavior.ClientCascade);

            builder.HasOne(ce => ce.Service)
                .WithMany(s => s.CartedByUsers) 
                .HasForeignKey(ce => ce.ServiceId)
                .OnDelete(DeleteBehavior.ClientCascade); 

            builder.ToTable("Carts");
        }
    }
}

