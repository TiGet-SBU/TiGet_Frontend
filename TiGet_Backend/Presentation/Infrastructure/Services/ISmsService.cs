using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rhazes.Services.Identity.API.Infrastructure.Services
{
    public interface ISmsService
    {
        Task<bool> SendAsync(string phoneNumber, string body);
    }
}
