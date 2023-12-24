using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public record Ticket : BaseEntity
    {
        public required DateTime TimeToGo { get; set; }
        public required double Price { get; set; }

        public required Guid VehicleId { get; set; }
        public Vehicle? Vehicle { get; set; }

        public required Guid CompanyId { get; set; }
        public Company? Company{ get; set;}

        public required Guid SourceId { get; set; }
        public required Station Source { get; set; }

        public required Guid DestinationId { get; set; }
        public required Station Destination { get; set; }

    }
}
