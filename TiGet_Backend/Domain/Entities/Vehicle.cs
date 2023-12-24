using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public record Vehicle : BaseEntity
    { 
        public string Name { get; set; } = string.Empty;
        public required VehicleType Type { get; set; }
        public required int Capacity { get; set; }
        
    }
}
