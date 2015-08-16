using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Access.Models
{
    public class Center:BaseModel
    {
        public Center()
        {
            CreateDate = DateTime.Now;
            CreateTime = DateTime.Now.Hour;
        }

        public int? Idcompany { get; set; }

        [DisplayName("Nombre")]
        [StringLength(100)]
        public string Name { get; set; }

        [DisplayName("Tipo")]
        public string Type { get; set; }

        [DisplayName("Ubicacion")]
        [StringLength(200)]
        public string Location { get; set; }

        [DisplayName("Coordenadas")]
        [ScriptIgnore]
        public DbGeography Coordinates { get; set; }

        [DisplayName("Descripcion")]
        [StringLength(250)]
        public string Description { get; set; }

        [DisplayName("Cuidad")]
        public int? Town { get; set; }

        [DisplayName("Departamento")]
        public int? Department { get; set; }

        [DisplayName("Pais")]
        public int? Country { get; set; }

        [DisplayName("Barrio / Colonia")]
        [StringLength(100)]
        public string Neighborhood { get; set; }


        [DisplayName("Nombre Administrador")]
        [StringLength(50)]
        public string Administrator { get; set; }


        [DisplayName("Contacto")]
        [StringLength(50)]
        public string Personcontact { get; set; }


        [DisplayName("Telefono 1")]
        [StringLength(20)]
        public string Phone1 { get; set; }


        [DisplayName("Telefono 2")]
        [StringLength(20)]
        public string Phone2 { get; set; }

        [StringLength(50)]
        [EmailAddress]
        [DisplayName("Email ")]
        public string Email { get; set; }

        [DisplayName("Hora de Apertura")]
        public int Opentime { get; set; }

        [DisplayName("Hora de Cierre")]
        public int Closetime { get; set; }
         
        [DisplayName("Estado")]
        
        public Status Status { get; set; }

        [DisplayName("Usuario")]
        public int? UserSign { get; set; }

        [DisplayName("Fecha Creacion")]
       [Required]
        public DateTime CreateDate { get; set; }


        
        [DisplayName("Hora Creacion")]
        public int? CreateTime { get; set; }

        [Required]
        [StringLength(15)]
        public string OBJECTTYPE { get; set; }


        public List<Field> Fields { get; set; }

        public List<ImageField> ImageField { get; set; }

        public List<CenterAccount> Employees { get; set; }

        public List<Service> Services { get; set; }
    }
}
