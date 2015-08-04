using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access.Models;

namespace Access.Extensions
{
    public static class BookingExtensions
    {
        public static string GetstatusClassForButton(this Booking booking)
        {
            var date = DateTime.Now.Date;
            return booking.Isbusy ? "btn btn-default btn-sm"
                : (booking.IsNearTo &&
                    booking.Start.HasValue &&
                    booking.Start.Value.Date==date.Date ? "btn btn-warning btn-sm" : "btn btn-primary btn-sm");
        }

        public static DateTime EstimatedTime(DateTime? date,int hour, bool forPreview)
        {
            TimeSpan diff = DateTime.Now - DateTime.Now.Date.AddHours(hour+1);

            // Aproximacion de tiempo estimado para no mostrar tiempos cercanos a la hora de alquiler 

            var approxhour = (diff.Minutes >= 20 && diff.Minutes < 30)
                ? DateTime.Now.Date.AddHours(hour + 1)
                : DateTime.Now.Date.AddHours(hour);

            
            return  date.HasValue ?
                       RoundUp( date.Value,TimeSpan.FromMinutes(30)) :
                       (forPreview ? approxhour : DateTime.Now.Date);
        }

        public static  DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }

        public static  List<Booking> BuildTimes(FilterOptionModel filter, int limit = 4)
        {
            // TODO take user Hour
            
            var firstHour = (filter.date.HasValue ? RoundUp( filter.date.Value,TimeSpan.FromMinutes(60)) :  RoundUp( DateTime.Now,TimeSpan.FromMinutes(60)));
            DateTime time;
            if (DateTime.TryParse(filter.time, out time))
            {
                firstHour = new DateTime(firstHour.Year,firstHour.Month,firstHour.Day,time.Hour,time.Minute,time.Second);
            };

            var ranges = new List<Booking>();

            for (int index=0;index<limit;index++)
            {
                ranges.Add(new Booking()
                {
                    Start = firstHour,
                    End = firstHour= firstHour.AddHours(1),
                });
                
            }

            return ranges;
        }
    }
}
