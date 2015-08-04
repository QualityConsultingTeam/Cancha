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
    public class Schedule :BaseModel
    {

        public Schedule()
        {
             
        }

        [DisplayName("Nombre")]
        [StringLength(100)]
        public string Name { get; set; }


        [DisplayName("Tipo")]
        public int? Type { get; set; }

        [DisplayName("Descripcion")]
        [StringLength(250)]
        public string Description { get; set; }

        [DisplayName("Estado")]
        
        public Status Status { get; set; }

        [Required]

        public DateTime Date { get; set; }

        public Int64 ValidityPeriodTicks { get; set; }

        [NotMapped]
        public TimeSpan ValidityPeriod
        {
            get { return TimeSpan.FromTicks(ValidityPeriodTicks); }
            set { ValidityPeriodTicks = value.Ticks; }
        }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Required]
        [StringLength(15)]
        public string OBJECTTYPE { get; set; }


        public Booking Booking { get; set; }

        public Field Field { get; set; }

        public int FieldId { get; set; }
    }
}
