using Access.Repositories;
using BussinesAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Admin.Controllers
{
    public class  ApiPaymentController : BaseApiController<PaymentRepository, Access.AccessContext, Access.Models.Payment>
    {

        [HttpPost]
        [Route("api/Payment")]
        [ResponseType(typeof(PayPal.Api.Payment))]
        public async Task<IHttpActionResult> Payment(int bookingId)
        {
            var manager = new PaymentManager();
            var booking = await Repository.GetBookingAsync(bookingId);

            var payment = manager.GetPayment(booking);

            if (payment == null) return NotFound();

            booking.PaypalPaymentId = payment.id;

            await Repository.SaveAsync();

            return Ok(payment);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/Verify")]
        [ResponseType(typeof(PayPal.Api.Payment))]
        public async Task<IHttpActionResult> VerifyPayment(string paymentId, string token, string PayerID)
        {
            var manager = new PaymentManager();
            var paymentDone = manager.ConfirmPayment(paymentId, token, PayerID);

            if (paymentDone) await Repository.ConfirmPaypalPayment(paymentId);


            IHttpActionResult response;
            HttpResponseMessage responseMsg = new HttpResponseMessage(HttpStatusCode.RedirectMethod);
            responseMsg.Headers.Location = new Uri(string.Format("{0}", ConfigurationManager.AppSettings["clientApp"]));
            response = ResponseMessage(responseMsg);
            return response;
        }
    }
}
