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
    public class Cost:BaseModel
    {

        public Cost()
        {
            CreateDate = DateTime.Now;
            CreateTime = DateTime.Now.Hour;
            OBJECTTYPE = "1";

        }
       
        [DisplayName("Hora de Apertura")]
        
        public int? Opentime { get; set; }

        [DisplayName("Hora de Cierre")]
        
        public int? Closetime { get; set; }

        [DisplayName("Precio")]
        
        [DataType(DataType.Currency)]
        public decimal? Price { get; set; }

         
        [DisplayName("Anticipo")]
        [DataType(DataType.Currency)]
        public decimal? DownPayment { get; set; }

        [DisplayName("Dia de Semana")]
        public int? Dayofweek { get; set; }

        [DisplayName("Cancha")]
        public int? IdCancha { get; set; }

        [DisplayName("Usuario")]
        public Guid UserSign { get; set; }

        [DisplayName("Fecha Creacion")]
       [Required]
        public DateTime CreateDate { get; set; }

        [DisplayName("Hora Creacion")]
        public int CreateTime { get; set; }

        [Required]
        [StringLength(15)]
        public string OBJECTTYPE { get; set; }

       // public Field Field { get; set; }

        //[ForeignKey("Field")]
        //public int FieldId { get; set; }
    }
}
