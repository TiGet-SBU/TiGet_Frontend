using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public record Company : User
    {
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

    }
}
