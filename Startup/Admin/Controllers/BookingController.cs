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
using System.Globalization;
using System.Security.Claims;
using Access.Extensions;
using Identity;
using Microsoft.AspNet.Identity;
using PagedList;
using Newtonsoft.Json;

namespace Admin.Controllers
{
    //[Globalization]
    [Authorize]
    public class BookingController : BaseController<BookingRepository, AccessContext, Booking>
    {



        #region Grid Administracion de Reservas 

        private async Task<Center> GetCenterAsync()
        {
             
            var repo = new CenterRepository() { Context = Context };
            return await repo.GetCenter ();
        }

        // Booking Management Grid.
        [Authorize(Roles ="Admin,Manager")]
        public async Task<ActionResult> Manage()
        {
            return View(await GetCenterAsync());
        }
        /// <summary>
        /// Center Header, 
        /// </summary>
        /// <param name="centerRepository"></param>
        /// <returns></returns>
        private object await(CenterRepository centerRepository)
        {
            throw new NotImplementedException();
        }
 
        /// <summary>
        /// Ajax Async Search Grid.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<ActionResult> SearchAync(FilterOptionModel filter)
        {
             
            //var usersFiltered = await IdentityManagerService.FilterUsers(filter,Context);
            var model = await Repository.GetSummary(filter);
             
            ViewBag.FilterModel = filter.Serialize();

            return View("Partials/ManageGrid", model.ToPagedList(filter.page==0?1:filter.page, filter.Limit));
        }

        /// ipaged list implementation.                               

        public async Task<ActionResult> SearchBookings(string filterdata, int? page, string columnName, string sortOrder)
        {
            var filter = JsonConvert.DeserializeObject<FilterOptionModel>(filterdata);
            var model = await Repository.GetSummary(filter);

            filter.page = page != null ? (int)page : 1;

            ViewBag.FilterModel = JsonConvert.SerializeObject(filter);
            ViewBag.Filter = filter;

            if (!string.IsNullOrEmpty(columnName) && !string.IsNullOrEmpty(sortOrder))
            {
                ViewBag.CurrentColumnSort = columnName;
                ViewBag.SortOrder = sortOrder;
                return View("Partials/ManageGrid", model.OrderBy(columnName, sortOrder).ToPagedList(page ?? 1, filter.Limit));
            }

            return View("Partials/ManageGrid", model.ToPagedList(page ?? 1, filter.Limit));
        }

        [HttpPost]
        public async Task<ActionResult> Sort(string filter, string columnName, string sortOrder, string currentColumn, int? page)
        {
            var _filter = JsonConvert.DeserializeObject<FilterOptionModel>(filter);
            var model = await Repository.GetSummary(_filter);

            sortOrder = string.IsNullOrEmpty(sortOrder) ? "ASC" : sortOrder == "ASC" ? "DESC" : "ASC";

            if (currentColumn != null)
                sortOrder = columnName != currentColumn ? "ASC" : sortOrder;

            ViewBag.CurrentColumnSort = columnName;
            ViewBag.SortOrder = sortOrder;
            ViewBag.FilterModel = filter;

            return View("Partials/ManageGrid", model.OrderBy(columnName, sortOrder).ToPagedList(_filter.page, _filter.Limit));
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

        /// <summary>
        /// Summary In Modal View.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UserSummary(Guid id)
        {
            var model = await IdentityManagerService.GetUserSummaryAsync(Context, id.ToString());

            return View("Partials/UserSummary", model);
        }

        

        #endregion   Grid Administracion de Reservas

        #region Scheduler Functions

        [Globalization]
        [Authorize(Roles ="Admin,Manager")]
        public async Task<ActionResult> Calendar()
        {
            var repo = new FieldsRepository() { Context = Context };
            ViewBag.fields = await repo.GetFieldsFromCenterAsync();
            return View(await GetCenterAsync());
        }

        public async Task<ActionResult> AddOrUpdate(int? id = null, string begin = "", string finish = "")
        {
            var model = id.HasValue ? await Repository.FindByIdAsync(id) : new Booking();
            if (!string.IsNullOrEmpty(begin) && !string.IsNullOrEmpty(finish))
            {
                // TODO parse Strings
                model.Start = DateTimeExtensions.ParseFromString(begin);
                model.End = DateTimeExtensions.ParseFromString(finish);
            }
            var repo = new FieldsRepository() { Context = Context };
            var fields = (await repo.GetFieldsFromCenterAsync());

            ViewBag.Fields = fields.ToSelectListItems(f => f.Name, f => f.Id.ToString(), model.Idcancha.ToString());

            return View("Partials/AddOrUpdate", model);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> AddOrUpdate(Booking booking)
        {
            if (ModelState.IsValid)
            {
                Repository.InsertOrUpdate(booking);
                await Repository.SaveAsync();
                return RedirectToAction("Calendar");
            }
            var repo = new FieldsRepository() { Context = Context };
            var fields = (await repo.GetFieldsFromCenterAsync());

            ViewBag.Fields = fields.ToSelectListItems(f => f.Name, f => f.Id.ToString(), booking.Idcancha.ToString());
            return View("Partials/AddOrUpdate", booking);
        }


        [Globalization]
        public virtual async Task<JsonResult> Read([DataSourceRequest] DataSourceRequest request)
        {

            var q = await Repository.GetSummary(onlyAvailables: true);

            IQueryable<BookingViewModel> query =q.Select(b => new BookingViewModel()
                {
                    Id = b.Id,
                    BookingId = b.Id,
                    Title = b.Field.Name,
                    Start =  b.Start.Value ,
                    End =  b.End.Value,
                    Userid = b.Userid,
                    Description = "",
                    Idcancha = b.Idcancha,
                }).AsQueryable();
           
            var model = query.ToDataSourceResult(request);

            var identityContext = Request.GetOwinContext().Get<AccessContext>();

            model.Data = await (model.Data as List<BookingViewModel>).UpdateAccountInfoFoScheduler(identityContext);

            return Json(model); 

        }

        public virtual async Task<JsonResult> Destroy([DataSourceRequest] DataSourceRequest request, BookingViewModel task)
        {

            if (ModelState.IsValid)
            {
                var booking = await Repository.FindByIdAsync(task.BookingId);

                if (booking.Status == BookingStatus.Pendiente)
                {
                    Repository.Delete(booking);

                    await Repository.SaveAsync();

                    return Json(new[] { task }.ToDataSourceResult(request, ModelState));
                }
                return Json(null);
            }
            return Json(null);
            
        }
        [Globalization]
        public virtual async Task<JsonResult> Create([DataSourceRequest] DataSourceRequest request, BookingViewModel task)
        {
            try {

                var user = await IdentityManagerService.GetUserAsync(task.UserKey);
                task.Title = user.FirstName+" - "+user.Email;
                
                var booking = new Booking();
                
                booking.CopyFrom(task);
                booking.Price = task.ComputePrice();
                booking.Start = task.Start.ToLocalTime();
                booking.End = task.End.ToLocalTime();
                Repository.InsertOrUpdate(booking);
                await Repository.SaveAsync();
                return Json(new[] { task }.ToDataSourceResult(request, ModelState));
            }
            catch(Exception ex)
            {
                return Json(null);
            } 
        }

        public virtual async Task<JsonResult> Update([DataSourceRequest] DataSourceRequest request, BookingViewModel task)
        {
            if (ModelState.IsValid)
            {
                var booking = new Booking();
                booking.CopyFrom(task);
                  Repository.InsertOrUpdate(booking);
                await Repository.SaveAsync();

            }

            return Json(new[] { task }.ToDataSourceResult(request, ModelState));
        }

        #endregion

        #region Grid functions

        // GET: Bookikng
        /// <summary>
        /// Grid Kendo Administracion de reservas
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        protected IdentityManagerService IdentityManagerService
        {
            get { return new IdentityManagerService(Request.GetOwinContext().Get<AccessContext>()); }
        }
         
         
        public async Task<ActionResult> DataRead([DataSourceRequest] DataSourceRequest request)
        {
            IQueryable<Booking> result = await Repository.GetSummary();

            var model = result.ToDataSourceResult(request);

            model.Data = await IdentityManagerService.UpdateAccountInfo(model.Data as List<Booking>);
            return Json( model, JsonRequestBehavior.AllowGet);
        }

       

        public async Task<ActionResult> UpdateBookingStatus(int id,BookingStatus status)
        {
            ViewBag.ActionMessage = Repository.MessageForStatus(status);
            var model = await Repository.FindByIdAsync(id, "Field","User");
            model.Status = status;

            if(model.Status == BookingStatus.Reservada) Task.Run(async () => await UserManager.SendEmailAsync(model.User.Id, "Confirmacion Reserva", model.GetMessageForBookingConfirmation()));


            return View("Partials/ConfirmBookingAction", model);
        }
 

        [HttpPost]
        public async Task<ActionResult> DoConfirmBookingAction(Booking booking)
        {
             
            Repository.InsertOrUpdate(booking); 
            await Repository.SaveAsync();

            await Repository.UpdateAccountLevel(booking);

            return Json(booking.Status.ToString().ToUpper(), JsonRequestBehavior.AllowGet);
            //return View("Partials/ConfirmBookingAction", await Repository.FindByIdAsync(booking.Id, "Field"));

        } 

        #endregion
    }
}