using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models
{
    public class UserInfo
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public int ShedulesCount { get; set; }

        [DisplayName("Telefono")]
        public string Phone { get; set; }

        [DisplayName("")]
        public string Phone2 { get; set; }

        [DisplayName("Reservas :")]
        public List<BookingSummary> BookingSummary { get; set; }

        public string Comment { get; set; }
    }

    public class BookingSummary
    {
        public string Label { get; set; }

        public int Count { get ; set; }
    }
    
}
