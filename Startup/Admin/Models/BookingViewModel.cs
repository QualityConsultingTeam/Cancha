using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access.Models;
using Kendo.Mvc.UI;

namespace Admin.Models
{
    public class BookingViewModel : Booking, ISchedulerEvent 
    {
       

        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsAllDay { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }

        public int RecurrenceId { get; set; }
    }
}
