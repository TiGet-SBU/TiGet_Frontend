using System.ComponentModel.DataAnnotations;

namespace Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "نام کاربری")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }

        [Display(Name = "مرا به خاطر داشته باش")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}