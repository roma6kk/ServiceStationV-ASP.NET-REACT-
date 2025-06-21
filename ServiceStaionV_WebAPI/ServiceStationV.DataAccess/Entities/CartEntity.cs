using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationV.DataAccess.Entities
{
    public class CartEntity
    {
        public Guid UserId { get; set; }
        public UserEntity User { get; set; }

        public Guid ServiceId { get; set; }
        public ServiceEntity Service { get; set; }
    }
}
