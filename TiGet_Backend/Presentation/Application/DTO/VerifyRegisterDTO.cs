using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Application.DTO
{
    public class VerifyRegisterDTO
    {
        public Guid UserId { get; set; }
        public bool Verified { get; set; }
    }
}
