using System.Collections.Generic;

namespace ServiceStationV.DataAccess.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public ICollection<UserFavouriteEntity> FavouriteList { get; set; } = new List<UserFavouriteEntity>();
        public ICollection<CartEntity> Cart { get; set; } = new List<CartEntity>();

    }
}