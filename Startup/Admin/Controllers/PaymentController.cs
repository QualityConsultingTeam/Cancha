using Access;
using Access.Models;
using Access.Repositories;
using BussinesAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class PaymentController : BaseController<PaymentRepository, AccessContext, Payment>
    {
        // GET: Payment
        public async Task<ActionResult> Pay(int id)
        {
            var manager = new PaymentManager();

            var booking = await Repository.GetBookingAsync(id);

            var payment = manager.GetPayment(booking);

            var url = payment.links.FirstOrDefault(p => p.rel == "");

            return Redirect(url.href);
        }
    }
}