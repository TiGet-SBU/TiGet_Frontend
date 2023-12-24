using Domain.Common;
using Domain.Enums;
using Domain.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public record Station : BaseEntity
    {
        public required Guid CityId { get; set; }
        public City? City { get; set; }
        public Location Location { get; set; }
        public required VehicleType vehicleType { get; set; } 
    }
}
