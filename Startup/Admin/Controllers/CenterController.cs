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
            var model = await Repository.FindByIdAsync(id, "Fields", "ImageField");
            return View(model);
        }

        // GET: Center
        public async Task<ActionResult> Index()
        {
            var model = await Repository.SearchAsync(new FilterOptionModel());

            return View(model);
        }

        // GET: Center/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Center/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Center/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Center/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Center/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Center/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Center/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
