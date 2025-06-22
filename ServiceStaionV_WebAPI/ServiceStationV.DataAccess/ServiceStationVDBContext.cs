using Microsoft.EntityFrameworkCore;
using ServiceStationV.DataAccess.Configurations;
using ServiceStationV.DataAccess.Entities;

namespace ServiceStationV.DataAccess
{
    public class ServiceStationVDBContext : DbContext
    {
        public ServiceStationVDBContext(DbContextOptions<ServiceStationVDBContext> options) : base(options)
        {

        }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ServiceEntity> Services { get; set; }
        public DbSet<UserFavouriteEntity> UserFavourites { get; set; }
        public DbSet<CartEntity> Carts { get; set; }

        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderServiceEntity> OrderServices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserFavouritesConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ServiceConfiguration());
            modelBuilder.ApplyConfiguration(new CartConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderServiceConfiguration());

        }

    }
}
