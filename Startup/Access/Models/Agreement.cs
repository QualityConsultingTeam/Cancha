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
    public  class Agreement: TrackedEntity
    {

        public Agreement()
        {
            CreateDate = DateTime.Now;
            CreateTime = DateTime.Now.Hour;
        }

        [DisplayName("Nombre")]
        [StringLength(100)]
        public string Name { get; set; }

        [DisplayName("Tipo")]
        public int? Type { get; set; }

        [DisplayName("Descripcion")]
        [StringLength(250)]
        public string Description { get; set; }

        [StringLength(250)]
        [DisplayName("Comentarios")]
        public string Comments { get; set; }

        [DisplayName("Fecha Inicio")]
        public DateTime? Start { get; set; }

        
        [DisplayName("Fecha Final")]
        public DateTime? End { get; set; }

        [DisplayName("Estado")]
        public Status Status { get; set; }


        //public Guid UserSign { get; set; }

        [DisplayName("Fecha Creacion")]
       [Required]
        
        public DateTime CreateDate { get; set; }

        [DisplayName("Hora Creacion")]
        
        
        public int  CreateTime { get; set; }

        [Required]
        [StringLength(15)]
        public string OBJECTTYPE { get; set; }
    }
}
