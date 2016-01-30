using Access.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models
{
   public class BookingManageViewModel
    {
        public int Id { get; set; }

      

    
        [DisplayName("Cancha")]
        public string FieldName { get; set; }

        
        [DisplayName("Inicio")]
        public DateTime? Start { get; set; }

        [DisplayName("Final")]
        public DateTime? End { get; set; }

        

        [DisplayName("Cancha")]
        public int? FieldId { get; set; }

       
        public string Userid { get; set; }

        public string UserEmail { get; set; }

        public string UserFullName { get; set; }


        public BookingType Type { get; set; }

        public BookingStatus Status { get; set; }
      
    }
}

