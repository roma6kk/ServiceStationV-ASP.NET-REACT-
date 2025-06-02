using Microsoft.EntityFrameworkCore;
using ServiceStationV.DataAccess.Entities;
namespace ServiceStationV.DataAccess
{
    public class ServiceStationVDBContext : DbContext
    {
        public ServiceStationVDBContext(DbContextOptions<ServiceStationVDBContext> options) : base(options)
        {

        }
        public DbSet<ServiceEntity> Services { get; set; }
    }
}
