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
    public class Booking : TrackedEntity
    {
       
        public Booking()
        {
            CreateDate = DateTime.Now;
            CreateTime = DateTime.Now.Hour;
            Start = DateTime.Now;
            End = DateTime.Now;
            UserInfo = new UserInfo();
            OBJECTTYPE = "1";
        }


        [DisplayName("Usuario")]
        public Guid Userid { get; set; }

        [DisplayName("Cancha")]
        public int? Idcancha { get; set; }

        [DisplayName("Inicio")]
        public DateTime? Start { get; set; }

        [DisplayName("Final")]
        public DateTime? End { get; set; }

        [DisplayName("Numero Factura")]
        [StringLength(25)]
        public string Invoice { get; set; }

        [DisplayName("Estado")]
        public BookingStatus Status { get; set; }

        [DisplayName("Precio")]
         [DataType(DataType.Currency)]
        public decimal? Price { get; set; }


        [DisplayName("Anticipo")]
        [DataType(DataType.Currency)]
        public decimal? DownPayment { get; set; }

        [DisplayName("Tiene Anticipo")]
        public bool? HasDownpay { get; set; }

        [DisplayName("Monoto Pendiente")]
        [DataType(DataType.Currency)]
        public decimal? Pending { get; set; }

        [DisplayName("Equipo 1")]
        [StringLength(50)]
        public string Team1 { get; set; }

        [DisplayName("Equipo 2")]
        [StringLength(50)]
        public string Team2 { get; set; }

        //[DisplayName("Usuario")]
        //public string UserSign { get; set; }

        [Required]
        [DisplayName("Fecha")]
        public DateTime CreateDate { get; set; }

         [DisplayName("Hora Creacion")]
         
        public int? CreateTime { get; set; }


         [Timestamp]
         public byte[] RowVersion { get; set; }

        [DisplayName("Tipo")]
         public BookingType Type { get; set; }



        [Required]
        [StringLength(15)]
        public string OBJECTTYPE { get; set; }

        // ID cancha Foreign key
        public Field Field { get; set; }

        

        // no mapeada aun. 
        [NotMapped]
        public Schedule Schedule { get; set; }

        // to identitfy if exists on database
        [Display(Name = "Ocupado",Prompt = "Ocupado")]
        public bool Isbusy
        {
            get
            {
                return Id>0;
            }
        }

        public bool IsNearTo
        {
            get
            {
                if (!Start.HasValue) return false;
                var date =  DateTime.Now;
                TimeSpan time = Start.Value - date;

                return time.Hours == 0 && time.Minutes < 30;
            }
        }


        [NotMapped]
        public UserInfo UserInfo { get; set; }

    }
}
