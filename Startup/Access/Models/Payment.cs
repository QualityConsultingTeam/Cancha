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
    public class Payment : BaseModel    
    {


        public Payment()
        {
            CreateDate = DateTime.Now;
            CreateTime = DateTime.Now.Hour;
            Docdate = DateTime.Now;
            Doctime = DateTime.Now.Hour;
            OBJECTTYPE = "1";
        }

        [DisplayName("Fecha Doc")]
        public DateTime? Docdate { get; set; }

        [DisplayName("Hora Doc")]
        public int? Doctime { get; set; }

        [DisplayName("Usuario")]
        public Guid Userid { get; set; }

        [DisplayName("Descripcion")]
        [StringLength(250)]
        public string Description { get; set; }

        [DisplayName("Comentarios")]
        [StringLength(250)]
        public string Comments { get; set; }

        [DisplayName("Monto")]
        [Column(TypeName = "numeric")]
        [DataType(DataType.Currency)]
        public decimal? Amount { get; set; }

        [DisplayName("Gestor")]
        public Guid? Usersign { get; set; }

        [DisplayName("Fecha Creacion")]
       [Required]
        public DateTime CreateDate { get; set; }


        [DisplayName("Hora Creacion")]
        
        public int? CreateTime { get; set; }


        [Timestamp]
        public byte[] RowVersion { get; set; }


        [Required]
        [StringLength(15)]
        public string OBJECTTYPE { get; set; }
    }
}
