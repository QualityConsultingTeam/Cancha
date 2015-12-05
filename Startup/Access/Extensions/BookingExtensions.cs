using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access.Models;
using System.Data.Entity;

namespace Access.Extensions
{
    public static class BookingExtensions
    {
        public static string GetstatusClassForButton(this Booking booking)
        {
            /// la propiedad isNearTo se usa para saber si la hora es cercana a la hora actual 
            /// tipo para no reservar la cancha menos de una hora si ya es tarde 
            var date = DateTime.Now.Date;
            return booking.Isbusy ? "btn btn-default btn-sm"
                : (booking.IsNearTo &&
                    booking.Start.HasValue &&
                    booking.Start.Value.Date==date.Date ? "btn btn-primary btn-sm" : "btn btn-primary btn-sm");
        }

        /// <summary>
        ///  Round up Stimated time
        /// </summary>
        /// <param name="date"></param>
        /// <param name="hour"></param>
        /// <param name="forPreview"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Round Up Stimated Time
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static  DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime(((dt.Ticks + d.Ticks - 1) / d.Ticks) * d.Ticks);
        }

        /// <summary>
        /// Create Times Ranges for Bookings Logic
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static  List<Booking> BuildTimes(FilterOptionModel filter, int limit = 4)
        {
            // TODO take user Hour
            
            var firstHour = (filter.date.HasValue ? RoundUp( filter.date.Value,TimeSpan.FromMinutes(60)) :  RoundUp( DateTime.Now,TimeSpan.FromMinutes(60)));
            DateTime time;
            if (DateTime.TryParse(filter.time, out time))
            {
                firstHour = new DateTime(firstHour.Year,firstHour.Month,firstHour.Day,time.Hour,time.Minute,time.Second);
                firstHour = RoundUp(firstHour, TimeSpan.FromMinutes(60));
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

        public static async Task<bool> UpdateUserAccountLevel(this Booking booking, AccessContext Context, bool ignoreStatus = false)
        {
            if ( (booking.Status == BookingStatus.Reservada|| booking.Status == BookingStatus.Pendiente) || ignoreStatus)
            {
                var center = await Context.Centers.Where(c => c.Fields.Any(f => f.Id == booking.Idcancha))
                                                .FirstOrDefaultAsync();

                if (!await Context.AccountAccess.AnyAsync(c => c.UserId == booking.Userid.ToString()))
                {
                    Context.AccountAccess.Add(new AccountAccessLevel()
                    {
                        UserId = booking.Userid.ToString(),
                        Center = center,
                    });
                    await Context.SaveChangesAsync();
                }
                return true;
            }
            return false;
        }


       
    }
}
