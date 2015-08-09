using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access.Models;
using Kendo.Mvc.UI;
using System.ComponentModel;

namespace Admin.Models
{
    public class BookingViewModel : ISchedulerEvent 
    {

        [DisplayName("Titulo")]
        public string Title { get; set; }

        [DisplayName("Descripcion")]
        public string Description { get; set; }

        [DisplayName("Dia Completo")]
        public bool IsAllDay { get; set; }

        [DisplayName("Inicio")]
        public DateTime Start { get; set; }

        [DisplayName("Final")]
        public DateTime End { get; set; }

        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }

        public int RecurrenceId { get; set; }
        public int Id { get; internal set; }
        public Guid UserId { get; internal set; }

        public UserInfo UserInfo { get; set; }
    }
}
