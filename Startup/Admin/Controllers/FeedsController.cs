using Access;
using Access.Extensions;
using Access.Models;
using Access.Repositories;
using Admin.Helpers;
using BussinesAccess.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class FeedsController : BaseController<FeedsRepository,AccessContext, Feed>
    {
        // GET: Feeds       
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> Index()
        {
            var model = await Repository.GetCompanyAsync();
            return View(model);
        }
        #region Grid admin  Feeds
       [Globalization]

        public async Task<ActionResult> Edit(int? id)
        {
            var model = id.HasValue ? (await Repository.FindByIdAsync(id.Value)) : new Feed();

            ViewBag.Categories = (await Repository.GetCategoriesAsync()).ToSelectList(m => m.Name, m => m.Id.ToString(), "Select Category");
            //var feedStatus = Enum.GetValues(typeof(FeedStatus)).Cast<FeedStatus>()
            // .Select(i => new SelectListItem()
            // {
            //     Text = i.ToString(),
            //     Value = i.ToString(),
            // }).ToList();
            //ViewBag.Statuses = feedStatus;// feedStatus.ToSelectList(m=>m.Name, m => m.Id.ToString(), "Select Status");

            return View(model);
        }
       [Globalization]
        [HttpPost]
        public async Task<ActionResult> Edit(Feed model)
        {
            if (ModelState.IsValid)
            {
                Repository.InsertOrUpdate(model);

                await Repository.SaveAsync();

                return RedirectToAction("Index");
            }
            ViewBag.Categories = (await Repository.GetCategoriesAsync()).ToSelectList(m => m.Name, m => m.Id.ToString(), "Select Category");

            return View(model);
        }

        public async Task<ActionResult> Details(int id)
        {
            var model = await Repository.FindByIdAsync(id, "Category", "User");


            return View("Partials/Details", model);
        }



        /// <summary>
        /// Ajax Async Search Grid.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<ActionResult> SearchAync(FilterOptionModel filter)
        {
            filter.centerid = ClaimsPrincipal.Current.CenterId();

            //var usersFiltered = await IdentityManagerService.FilterUsers(filter,Context);
            var model = await Repository.GetSummaryAsync(filter);
            ViewBag.PageLimit = await Repository.GetPageLimit(filter);

            return View("Partials/ManageGrid", model);
        }

        public ActionResult Statuses()
        {
            var model = Enum.GetValues(typeof(FeedStatus)).Cast<FeedStatus>()
             .Select(i => new
             {
                 Id = (int)i,
                 Name = i.ToString(),
             }).ToList();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> UpdateStatus(int id, FeedStatus status)
        {
            // ViewBag.ActionMessage = Repository.MessageForStatus(status);
            var model = await Repository.FindByIdAsync(id, "User", "Category");
            model.Status = status;
            await Repository.SaveAsync();
            //return View("Partials/ConfirmAction", model);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public async Task<ActionResult> sendpush(int id)
        {
            var model = await Repository.FindByIdAsync(id, "Category");

            await NotificationsManager.SendNotificationAsync($"{model.Title} {model.Category.Name}");
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        ///// <summary>
        ///// Summary In Modal View.
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpPost]           u
        //public async Task<ActionResult> UserSummary(Guid id)
        //{
        //    var model = await Repository.GetUserSummaryAsync(id);

        //    return View("Partials/UserSummary", model);
        //}


        public async Task<ActionResult> Delete(int id)
        {
            var model = await Repository.FindByIdAsync(id);

            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> Delete(int id, FormCollection collection)
        {

            await Repository.DeleteByIdAsync(id);

            return RedirectToAction("Index");
        }

        #endregion   Grid  admin feed  
    }
}