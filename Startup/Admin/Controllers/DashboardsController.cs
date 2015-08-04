using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Access;
using Access.Models;

namespace Admin.Controllers
{

   // [Authorize]
    public class DashboardsController : BaseController<FieldsRepository,AccessContext,Field>
    {
        public ActionResult Dashboard_1()
        {
            return View();
        }

        //[Authorize(Roles = "Admin")]
        public ActionResult Dashboard_2()
        {
            return View();
        }

        public ActionResult Dashboard_3()
        {
            return View();
        }
        
        public async Task<ActionResult> Dashboard_4()
        {
            var models = await Repository.SearchAsync("");
            return View(models);
        }
        
        public ActionResult Dashboard_4_1()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> SeachFields(string keywords)
        {
            var model = await Repository.SearchAsync(keywords);

            return View(model);
        }

    }
}