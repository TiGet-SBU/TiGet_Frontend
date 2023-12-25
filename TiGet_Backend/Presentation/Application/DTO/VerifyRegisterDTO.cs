using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.API.Application.DTO
{
    public class VerifyRegisterDTO
    {
        public Guid UserId { get; set; }
        public bool Verified { get; set; }
    }
}
