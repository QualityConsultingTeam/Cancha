using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Spatial;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel;
using System.Data.Entity.ModelConfiguration;
using Access.Models;
using System.Collections.Generic;

namespace Identity.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }

        public ApplicationUser()
        {
            CreatedDate = DateTime.Now;
            Feeds = new List<Feed>();
        }

        public ApplicationUser(string email, string name, string lastname, string doc = "")
        {
            this.Email = this.UserName = email;
            this.FirstName = name;
            this.LastName = lastname;
            DUI = doc;
            CreatedDate = DateTime.Now;
        }

        [DisplayName("Nombre")]
        public string FirstName { get; set; }


        [DisplayName("Apellido")]
        public string LastName { get; set; }

        [StringLength(25)]
        public string DUI { get; set; }

        [StringLength(20)]
        public string PHONE_2 { get; set; }

        [StringLength(200)]
        public string ADDRESS { get; set; }

        public DbGeography Location { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha Creacion")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Calificacion:", Prompt = "Categoria")]
        [Range(1, 5, ErrorMessage = "Debe especificar un maximo 5")]
        public decimal? Category { get; set; }

        //[Timestamp]
        //public byte[] RowVersion { get; set; }
        [DisplayName("Profile Picture")]
        public string ProfilePicture { get; set; }

        [Display(Name = "Complejo")]
        public int? CenterId { get; set; }

        public Center Company { get; set; }


        public List<Feed> Feeds { get; set; }

    }


}