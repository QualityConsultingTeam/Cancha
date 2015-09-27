using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Admin.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage ="El {0} l es requerido")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage =" El {0} no es valido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El {0} es Requerido")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Recordarme?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La {0} Debe Contener como minimo {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "La de la contraseña Confirmacion no coincide.")]
        public string ConfirmPassword { get; set; }

        [Display(Name ="Telefono",Prompt ="Telefono Móvil")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
    

    //public class IdentityUserViewModel
    //{

    //    public string Id { get; set; }

    //    public string Email { get; set; }

    //    [DisplayName("Telefono")]
    //    public string PHONE_2 { get; set; }

    //    public int AccessFailedCount { get; set; }

    //    public string Role { get; set; }

        
    //    public string Password { get; set; }

    //    [DisplayName("Change Password")]
    //    public bool ForceChangePassword { get; set; }

    //    [DisplayName("Nombre")]
    //    public string FirstName { get; set; }


    //    [DisplayName("Apellido")]
    //    public string LastName { get; set; }

    //    //[StringLength(25)]
    //    public string DUI { get; set; }

    //    //[StringLength(20)]
    //    //public string PHONE_2 { get; set; }

    //    [StringLength(200)]
    //    [DisplayName("Direccion")]
    //    public string ADDRESS { get; set; }

    //    [Required]
    //    [DisplayName("User Name")]
    //    public string UserName { get; set; }

    //    [DisplayName("Imagen de Perfil")]
    //    public string ProfilePicture { get; set; }

    //    public string ProfilePictureDefault
    //    {
    //        get { return !string.IsNullOrEmpty(ProfilePicture) ? ProfilePicture : "/Images/profile.jpg"; }
    //    }

    //    //public List<IsInRole> OnRoles { get; set; } 

    //    [Display(Name = "Calificacion:", Prompt = "Categoria")]
    //    [Range(1, 5, ErrorMessage = "Debe especificar un maximo 5")]
    //    public decimal? Category { get; set; }

    //    public dynamic WorkSummary { get; set; }

    //    [Display(Name ="Complejo")]
    //    public int? CenterId { get; set; }

    //    [DisplayName("Nombre de Complejo")]
    //    public string CenterSearchName { get; set; }

    //    public bool DisableForCenter { get; set; }

    //}
}
