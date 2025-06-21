using System.Collections.Generic;

namespace ServiceStationV.DataAccess.Entities
{
    public class ServiceEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImagePath { get; set; } = string.Empty;

        public ICollection<UserFavouriteEntity> FavouritedByUsers { get; set; } = new List<UserFavouriteEntity>();
        public ICollection<CartEntity> CartedByUsers { get; set; } = new List<CartEntity>();

    }

}