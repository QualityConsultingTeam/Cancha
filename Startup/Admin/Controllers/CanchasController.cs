using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Access;
using Access.Extensions;
using Access.Models;
using Microsoft.AspNet.Identity;

namespace Admin.Controllers
{
    [RequireHttps]
    public class CanchasController : BaseController<FieldsRepository,AccessContext,Field>
    {
        // GET: Canchas
        public  ActionResult Index()
        {
            ViewBag.lstFields = "hola";
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SearchFields(FilterOptionModel filter)
        {
            var model = await Repository.FullSearchAsync(filter);
             ViewBag.Filter = filter;
            return View("Partials/SearchFields", model);
        }

        [HttpPost]
        public async Task<ActionResult> FullSpecsInRange(int id,DateTime? date,DateTime? end,string distance)
        {
            ViewBag.Filter = new FilterOptionModel {date = date, end = end};
            var field = await Repository.FullSearchAsync(id, date,end,distance);
            
            return View("Partials/FullSpecs",field);
        }

        [HttpPost]
        public async Task<ActionResult> FullSpecs(int id)
        {
            ViewBag.Filter = new FilterOptionModel() {date = DateTime.Now.Date};
            var field = await Repository.FullSearchAsync(id, DateTime.Now, null, "");

            return View("Partials/FullSpecs", field);
        }
         
        
        [HttpPost]
        public async Task <ActionResult> CreateBook(Booking book)
        {

            book.Field = await Repository.FindByIdAsync(book.Idcancha.Value, "Center", "Cost");
            
            return View("Partials/CreateBook", book);
        }

        
        [HttpPost]
        public async Task<ActionResult> Confirmar(Booking booking)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("ConfirmaReserva",
                    new { fieldId = booking.Idcancha, start = booking.Start.Value.ToShortTimeString(), end = booking.End.Value.ToShortTimeString() });


            booking.Userid = LoggedUser.Value;
            booking.OBJECTTYPE = "1";
            Repository.AddOrUpdateBooking(booking);

            await Repository.SaveAsync(LoggedUser);


            return View(booking);
        }

        [Authorize]
        public async Task<ActionResult> Confirmar(int id)
        {
            var model = await Repository.FindBookAsync(id);

            return View(model);
        }

        [Authorize]
        public async  Task<ActionResult>ConfirmaReserva(int fieldId, string  start, string  end)
        {
            DateTime startDate;

            if (!DateTime.TryParse(start,out startDate)) return RedirectToAction("Index", "Canchas");

            DateTime endDate = startDate.AddHours(1);

            var model = new Booking
            {
                Field = await Repository.FindByIdAsync(fieldId, "Center", "Cost"),
                Idcancha = fieldId,
                Start = startDate,
                End = endDate,
                UserSign = LoggedUser.Value,
            };

            return View(model );
        }

 
    }
}