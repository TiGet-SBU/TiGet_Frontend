using System.ComponentModel.DataAnnotations;

namespace Rhazes.Services.PadidarServerIdentity.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
