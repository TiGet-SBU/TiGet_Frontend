using System;
using System.ComponentModel.DataAnnotations;

namespace Models.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        public Guid UserId { get; set; }
        public string Password { get; set; }
    }
}
