using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public record City
    {
        public required Guid Id { get; set; }
        public required string CityName { get; set; }
        public required string Province { get; set; }
    }
}
