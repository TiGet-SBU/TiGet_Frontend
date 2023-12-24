using Domain.Common;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public abstract record User : BaseEntity
    {
        public required Role Role { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;

    }
}
