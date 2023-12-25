using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models.AccountViewModels
{
    public class VerifyTwoFactorTokenViewModel
    {
        public string Token { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
    }
}
