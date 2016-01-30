using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access.Models;
using System.Data.Entity;
using Access.Extensions;

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

        public async Task ConfirmPaypalPayment(string paymentId)
        {
            var booking = await Context.Bookings
               .FirstOrDefaultAsync(b => b.PaypalPaymentId == paymentId);

            booking.PaypalPaymentCompleted = true;
            booking.Status = BookingStatus.Finalizado;

            await booking.UpdateUserAccountLevel(Context, ignoreStatus: true);

            await SaveAsync();

            var payment = new Access.Models.Payment()
            {
                 Amount = booking.Price,
                 Userid =new Guid( booking.Userid),
                 Description ="Pago A travez de Paypal",

            };

            Context.Payments.Add(payment);

            await SaveAsync();
        }
    }
}
