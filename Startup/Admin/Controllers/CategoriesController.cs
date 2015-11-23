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
    public class CategoriesController : BaseController<CategoriesRepository,AccessContext, Category>
    {
        // GET: Categories
        public async Task<ActionResult> Index()
        {
            var categories = await Repository.GetAllAsync();
            return View(categories);
        }

        // GET: Categories/Details/5
        public async Task<ActionResult> Details(int id)
        {
            return View(await Repository.FindByIdAsync(id));
        }



        // GET: Categories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            var model = id.HasValue ? ((await Repository.FindByIdAsync(id)) ?? new Category()) : new Category();
            return View(model);
        }

        // POST: Categories/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Category model)
        {
            if (ModelState.IsValid)
            {
                Repository.InsertOrUpdate(model);
                await Repository.SaveAsync();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Categories/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            return View(await Repository.FindByIdAsync(id));
        }

        // POST: Categories/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int id, FormCollection form)
        {
            await Repository.DeleteByIdAsync(id);

            return RedirectToAction("Index");
        }

    }
}