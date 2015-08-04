using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models
{
    public class Company :BaseModel
    {

        public Company()
        {
            CreateDate = DateTime.Now;
            CreateTime = DateTime.Now.Hour;
        }

        [DisplayName("Nombre")]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(20)]
        public string Nit { get; set; }

        [DisplayName("Telefono 1")]
        [StringLength(20)]
        public string Phone1 { get; set; }

        [DisplayName("Telefono 2")]
        [StringLength(20)]
        public string Phone2 { get; set; }

        [DisplayName("Direccion")]
        [StringLength(200)]
        public string Address { get; set; }

        [DisplayName("Persona de Contacto")]
        [StringLength(50)]
        public string Personcontact { get; set; }

        [DisplayName("Usuario")]
        public Guid UserSign { get; set; }

        [DisplayName("Fecha Creacion")]
       [Required]
        public DateTime CreateDate { get; set; }

        [DisplayName("Hora Creacion")]
        
        public int? CreateTime { get; set; }

        [DisplayName("Estado")]
        public Status Status { get; set; }

        [Required]
        [StringLength(15)]
        public string OBJECTTYPE { get; set; }
         
        
    }
}
