using ServiceStationV.DataAccess.Entities;

namespace ServiceStationV.DataAccess.Entities
{
    public class UserFavouriteEntity
    {
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }

        public Guid ServiceId { get; set; }
        public ServiceEntity Service { get; set; }
    }
}