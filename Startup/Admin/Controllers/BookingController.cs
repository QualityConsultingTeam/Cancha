using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Access;
using Access.Models;
using Access.Repositories;
using Admin.Helpers;
using Admin.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity.Owin;

namespace Admin.Controllers
{
    [Globalization]
    [Authorize]
    public class BookingController : BaseController<BookingRepository, AccessContext, Booking>
    {
        // GET: Bookikng
        public ActionResult Index()
        {
            return View();
        }


        public  ActionResult Manage()
        {
            return View();
        }

        public ActionResult Calendar()
        {
            return View();
        }

        public async Task<ActionResult> AddOrUpdate(int? id = null)
        {
            var model = id.HasValue ? await Repository.FindByIdAsync(id) : new Booking();

            return View("Partials/AddOrUpdate", model);
        }

        [HttpPost]

        public async Task<ActionResult> AddOrUpdate(Booking booking)
        {
            if (ModelState.IsValid)
            {
                Repository.InsertOrUpdate(booking);
                await Repository.SaveAsync(LoggedUser);
                return RedirectToAction("Manage");
            }
            return View("Partials/AddOrUpdate", booking);
        }


        public async Task<ActionResult> SearchAync(FilterOptionModel filter)
        {
            var model = await Repository.GetSummaryAsync(filter,LoggedUser.Value);

            return View("Partials/ManageGrid",  await IdentityManagerService.UpdateAccountInfo(model));
        }

        public ActionResult Statuses()
        {
            var model = Enum.GetValues(typeof(BookingStatus)).Cast<BookingStatus>()
             .Select(i => new
             {
                 Id = (int)i,
                 Name = i.ToString(),
             }).ToList();

            return Json(model, JsonRequestBehavior.AllowGet);
        }


        #region Scheduler Functions

        public virtual async Task<JsonResult> Read([DataSourceRequest] DataSourceRequest request)
        {

            IQueryable<BookingViewModel> query = Repository.GetSummary(LoggedUser.Value).ToList()
                .Select(b => new BookingViewModel()
                {
                    Id = b.Id,
                    Title = "Reserva de cancha",
                    Start = b.Start.ToSpecificKind(),
                    End = b.End.ToSpecificKind(),
                    UserId = b.Userid,
                    Description = "",
                }).AsQueryable();
           
            var model = query.ToDataSourceResult(request);

            
           model.Data = await IdentityManagerService.UpdateAccountInfoFoScheduler(model.Data as List<BookingViewModel>);

            return Json(model); 

        }

        public virtual async Task<JsonResult> Destroy([DataSourceRequest] DataSourceRequest request, BookingViewModel task)
        {
            if (ModelState.IsValid)
            {
                var booking = new Booking();
                booking.CopyFrom(task);
                Repository.Delete(booking);
                await Repository.SaveAsync(LoggedUser);
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        public virtual async Task<JsonResult> Create([DataSourceRequest] DataSourceRequest request, BookingViewModel task)
        {
            if (ModelState.IsValid)
            {
                var booking = new Booking();
                booking.CopyFrom(task);
                Repository.InsertOrUpdate(booking);
                await Repository.SaveAsync(LoggedUser);
                
            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        public virtual async Task<JsonResult> Update([DataSourceRequest] DataSourceRequest request, BookingViewModel task)
        {
            if (ModelState.IsValid)
            {
                var booking = new Booking();
                booking.CopyFrom(task);
                  Repository.InsertOrUpdate(booking);
                await Repository.SaveAsync(LoggedUser);

            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        #endregion

        #region Grid functions

        protected IdentityManagerService IdentityManagerService
        {
            get { return new IdentityManagerService(Request.GetOwinContext().Get<ApplicationDbContext>()); }
        }

         
        public async Task<ActionResult> DataRead([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<Booking> result = Repository.GetSummary(LoggedUser.Value);

            var model = result.ToDataSourceResult(request);

            model.Data = await IdentityManagerService.UpdateAccountInfo(model.Data as List<Booking>);
            return Json( model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BookingStates()
        {
            var states = Repository.Statuses.Select(st => new
            {
                id = (int) st,
                text = st.ToString(),
            });
            return Json(states, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> UpdateBookingStatus(int id,BookingStatus status)
        {
            ViewBag.ActionMessage = Repository.MessageForStatus(status);
            var model = await Repository.FindByIdAsync(id, "Field");
            model.Status = status;
            return View("Partials/ConfirmBookingAction", model);
        }
 

        [HttpPost]
        public async Task<ActionResult> DoConfirmBookingAction(Booking booking)
        {
             
            Repository.InsertOrUpdate(booking); 
            await Repository.SaveAsync(LoggedUser); 

            return Json(booking.Status.ToString().ToUpper(), JsonRequestBehavior.AllowGet);
            //return View("Partials/ConfirmBookingAction", await Repository.FindByIdAsync(booking.Id, "Field"));

        }

        [HttpPost]
        public async Task<ActionResult> UserSummary(Guid id)
        {
            var model =  await IdentityManagerService.GetUserSummary(Context, id);

            return View("Partials/UserSummary", model);
        }

 
        #endregion
    }
}