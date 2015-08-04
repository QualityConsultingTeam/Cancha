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
    public class Message :BaseModel
    {

        public Message()
        {
            CreateDate = DateTime.Now;
            CreateTime = DateTime.Now.Hour;
        }


        [DisplayName("Estado")]
        public Status Status { get; set; }

        [DisplayName("Usuario")]
        public Guid Usersign { get; set; }

        [DisplayName("Fecha Creacion")]
       [Required]
        public DateTime CreateDate { get; set; }

        [DisplayName("Hora Creacion")]
  
        public int? CreateTime { get; set; }

        [DisplayName("Accion")]
        public ActionTypes? Action { get; set; }

        [DisplayName("Accion")]
        public int? Idcenter { get; set; }

        [DisplayName("Descripcion")]
        [StringLength(250)]
        public string Description { get; set; }

        [DisplayName("Comentarios")]
        [StringLength(250)]
        public string Comments { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }


        [Required]
        [StringLength(15)]
        public string OBJECTTYPE { get; set; }
    }
}
