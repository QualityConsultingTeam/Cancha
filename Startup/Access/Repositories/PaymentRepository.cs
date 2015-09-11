using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access.Models;
using System.Data.Entity;

namespace Access.Repositories
{
    public  class PaymentRepository :BaseRepository<AccessContext,Access.Models.Payment>
    {
        public Task<Access.Models.Payment> GetPayment()
        {
            return null;   
        }

        public Task<Booking> GetBookingAsync(int bookingId)
        {
            return Context.Bookings
               .Include(b=>b.Field)
                .FirstOrDefaultAsync(b=>b.Id== bookingId);
        }
    }
}
