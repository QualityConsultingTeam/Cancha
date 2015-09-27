using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity
{
    public class IdentityUserViewModel
    {

        public string Id { get; set; }

        public string Email { get; set; }

        [DisplayName("Telefono")]
        public string PHONE_2 { get; set; }

        public int AccessFailedCount { get; set; }

        public string Role { get; set; }


        public string Password { get; set; }

        [DisplayName("Change Password")]
        public bool ForceChangePassword { get; set; }

        [DisplayName("Nombre")]
        public string FirstName { get; set; }


        [DisplayName("Apellido")]
        public string LastName { get; set; }

        //[StringLength(25)]
        public string DUI { get; set; }

        //[StringLength(20)]
        //public string PHONE_2 { get; set; }

        [StringLength(200)]
        [DisplayName("Direccion")]
        public string ADDRESS { get; set; }

        [Required]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [DisplayName("Imagen de Perfil")]
        public string ProfilePicture { get; set; }

        public string ProfilePictureDefault
        {
            get { return !string.IsNullOrEmpty(ProfilePicture) ? ProfilePicture : "/Images/profile.jpg"; }
        }

        //public List<IsInRole> OnRoles { get; set; } 

        [Display(Name = "Calificacion:", Prompt = "Categoria")]
        [Range(1, 5, ErrorMessage = "Debe especificar un maximo 5")]
        public decimal? Category { get; set; }

        public dynamic WorkSummary { get; set; }

        [Display(Name = "Complejo")]
        public int? CenterId { get; set; }

        [DisplayName("Nombre de Complejo")]
        public string CenterSearchName { get; set; }

        public bool DisableForCenter { get; set; }

    }

    public class SelectListModel<Tkey>
    {
        public Tkey Id { get; set; }

        public string Text { get; set; }
    }
}
