using System.ComponentModel.DataAnnotations;

namespace MVC_RBAC_Prctice.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter username/email")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}
