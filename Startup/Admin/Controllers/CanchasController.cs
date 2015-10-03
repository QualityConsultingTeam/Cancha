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
using Admin.Models;

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
        //[Globalization]
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
            return View("Partials/CreateBook", await Repository.FindWithPrice(book));
        }

        
        [HttpPost]
        public async Task<ActionResult> Confirmar(Booking booking)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("ConfirmaReserva",
                    new { fieldId = booking.Idcancha, start = booking.Start.Value.ToShortTimeString(), end = booking.End.Value.ToShortTimeString() });
            
            await Repository.AddOrUpdateBooking(booking);

            await Repository.SaveAsync();


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
                Idcancha = fieldId,
                Start = startDate,
                End = endDate,
                UserSign = new Guid( User.Identity.GetUserId ()),
            };

            model  = await Repository.FindWithPrice(model);
            
           // model.Price = model.Field.BestPrice.Price;

            return View(model );
        }

        #region AutoCompletes / Lista De Canchas en Scheduler
        [Authorize]
        public async Task<JsonResult> GetFieldsFromCenter(string keywords = "")
        {
            var model = (await Repository.GetFieldsFromCenterAsync())
                        .Select(f => new  { Id = f.Id, Text = f.Name }).ToList();
            return Json( model,JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public async Task<JsonResult> GetFieldsForAutoComplete(string query)
        {
            var model = await Repository.GetFieldsFromCenterAsync(keywords: query);

            return Json(model.ToAutocomplete(), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult PayPalCanceled()
        {
            return View();
        }
        #endregion  
    }
}