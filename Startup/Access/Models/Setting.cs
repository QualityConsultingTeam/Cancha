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
    public class Setting :BaseModel
    {

        public Setting()
        {
            CreateDate = DateTime.Now;
            CreateTime = DateTime.Now.Hour;
        }

        [DisplayName("Codigo")]
        [StringLength(10)]
        public string Code { get; set; }


        [DisplayName("Empresa")]
        public int? Idcompany { get; set; }

        [DisplayName("Tipo")]
        public int? Type { get; set; }

        [DisplayName("Descripcion")]
        [StringLength(250)]
        public string Description { get; set; }

        [DisplayName("Estado")]
        public Status Status { get; set; }

        [DisplayName("Usuario")]
        public int? Usersign { get; set; }

        [DisplayName("Fecha Creacion")]
       [Required]
        public DateTime CreateDate { get; set; }

        [DisplayName("Hora Creacion")]
       
        public int?CreateTime { get; set; }

        

        [Required]
        [StringLength(15)]
        public string OBJECTTYPE { get; set; }
    }
}
