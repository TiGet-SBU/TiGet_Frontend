using System;

namespace Identity.API.Application.DTO
{
    public class ResetPasswordDTO
    {
        public Guid UserId { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
