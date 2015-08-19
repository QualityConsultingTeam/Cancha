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
    ///[Globalization]
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
            var items = new List<List<Field>>();

            while (model.Any())
            {
                var subItems = model.Take(3).ToList();
                items.Add(subItems);
                subItems.ForEach(i => model.Remove(i));
            }
            
            return View("Partials/SearchFields", items);
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

       
        [Authorize]
        public async Task<JsonResult> GetFieldsFromCenter(int centerId,string keywords = "")
        {
            var model = (await Repository.GetFieldsFromCenterAsync(centerId, keywords))
                        .Select(f => new { Id = f.Id, Text = f.Name }).ToList();
            return Json( model,JsonRequestBehavior.AllowGet);
        }
    }
}