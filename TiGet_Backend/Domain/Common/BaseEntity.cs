using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
    public record BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
