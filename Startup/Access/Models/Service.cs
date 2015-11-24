using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models
{
     public class Service :BaseModel
    {
       
        [DisplayName("Nombre")]
        [StringLength(100)]
        public string Name { get; set; }

        [DisplayName("Id")]
        public int? IdCenter { get; set; }

        [DisplayName("Type")]
        public int? Type { get; set; }

        [StringLength(250)]
        [DisplayName("Descripcion")]
        public string Description { get; set; }

       
        [StringLength(250)]
        [NotMapped]
        [DisplayName("IconName")]
        public string IconName { get; set; }


        [DisplayName("Estado")]
        public Status Status { get; set; } 
         
        [Required]
        [StringLength(15)]
        public string OBJECTTYPE { get; set; }

        public Center Center { get; set; }
    }
}
