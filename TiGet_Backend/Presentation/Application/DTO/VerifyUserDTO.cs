using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Application.DTO
{
    public class VerifyUserDTO
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public string PhoneNumber { get; set; }
    }
}
