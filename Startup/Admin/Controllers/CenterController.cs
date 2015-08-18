using Access;
using Access.Models;
using Access.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Admin.Controllers
{
    public class CenterController : BaseController<CenterRepository,AccessContext,Center>
    {
        public async Task<ActionResult> CenterDetails(int id)
        {
            var model = await Repository.GetCenterDetailsAsync(id);
            return View(model);
        }

        // GET: Center
        public async Task<ActionResult> Index()
        {
            var model = await Repository.SearchAsync(new FilterOptionModel());

            return View(model);
        }

        

      
    }
}
