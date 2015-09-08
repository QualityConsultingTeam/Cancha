using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access.Models;
using Kendo.Mvc.UI;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Access.Extensions;

namespace Admin.Models
{
    public class BookingViewModel : ISchedulerEvent 
    {

        [DisplayName("Titulo")]
        public string Title { get; set; }

        public string FieldSearchName { get; set; }

        [DisplayName("Comentarios")]
        public string Description { get; set; }

        [DisplayName("Dia Completo")]
        public bool IsAllDay { get; set; }

        [DisplayName("Inicio")]
        public DateTime Start { get; set; }

        [DisplayName("Final")]
        public DateTime End { get; set; }

        public string StartString { get; set; }

        public string EndString { get; set; }

        [DisplayName("Cancha")]
        public int? Idcancha { get; set; }

        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }

        public int RecurrenceId { get; set; }
        public int Id { get; internal set; }

        [DisplayName("Cliente")]
        public Guid Userid
        {
            get
            {
                return _userId != Guid.Empty ? _userId
                     : (_userId = !string.IsNullOrEmpty(UserKey)? new Guid(UserKey) : Guid.Empty);
            }
            set
            {
                _userId = value;
            }
        }

        private Guid _userId { get; set; }

        [DisplayName("LLave Cliente")]
        public string UserKey
        {
            get;
            set;
        }

        

        public UserInfo UserInfo { get; set; }

        [DataType(DataType.Currency)]
        [DisplayName("Precio Promo")]
        public decimal ? CustomPrice { get; set; }

        [DisplayName("Descuento %")]
        public decimal ? Off { get; set; }


        public bool HasPromo
        {
            get { return CustomPrice.HasValue || Off.HasValue; }
        }

        internal decimal? ComputePrice()
        {
            if (CustomPrice.HasValue) return CustomPrice.Value;

            // Apply Discount
            return 0;
            
        }
    }
}
