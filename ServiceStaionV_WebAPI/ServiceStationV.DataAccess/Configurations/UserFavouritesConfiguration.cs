using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceStationV.DataAccess.Entities;

namespace ServiceStationV.DataAccess.Configurations
{
    public class UserFavouritesConfiguration : IEntityTypeConfiguration<UserFavouriteEntity>
    {
        public void Configure(EntityTypeBuilder<UserFavouriteEntity> builder)
        {
            builder.HasKey(ufe => new { ufe.UserId, ufe.ServiceId });

            builder.HasOne(ufe => ufe.User)
                .WithMany(u => u.FavouriteList)
                .HasForeignKey(ufe => ufe.UserId)
                .OnDelete(DeleteBehavior.ClientCascade); // Изменено на ClientCascade

            builder.HasOne(ufe => ufe.Service)
                .WithMany(s => s.FavouritedByUsers) // Теперь используем коллекцию
                .HasForeignKey(ufe => ufe.ServiceId)
                .OnDelete(DeleteBehavior.ClientCascade); // Изменено на ClientCascade

            builder.ToTable("UserFavourites");
        }
    }
}