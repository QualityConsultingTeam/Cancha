using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Access;
using Access.Models;
using Access.Repositories;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity;
using Admin.Models;

namespace Admin.Controllers
{
    
    //[Authorize]
    public class RootAdminController : BaseController<FieldsRepository,AccessContext, Field>
    {
        // GET: FieldsAdmin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Menu()
        {
            return View();
        }

        #region ProtectedRepositores 

        protected CenterRepository CenterRepository
        {
            get { return  new CenterRepository{Context = Context};}
        }

        protected CostRepository CostRepository
        {
            get{ return new CostRepository{Context = Context};}
        }

        protected CompanyRepository CompanyRepository
        {
            get { return new CompanyRepository(){Context = Context};}
        }

        #endregion

        #region JsonComboboxes

        public async Task<JsonResult> GetCountries(string query = "")
        {
            return Json(await Repository.GetCountriesAsync(query), JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetStates(int? idCountry, string statefilter="")
        {
            var states = await Repository.GetStatesAsync(idCountry, statefilter);
            return Json(states  , JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetCities(int? Department, string cityfilter)
        {
            var cities = await Repository.GetCitiesAsync(Department, cityfilter);
            return Json(cities, JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        #region Fields GridFuncitons

        public async Task<ActionResult> EditField(int id =0)
        {
            var model = id!=0? await Repository.FindByIdAsync(id) : new Field();
            return View(model);
        }

        
         
        [HttpPost]
        public async Task<ActionResult> EditField(Field field)
        {
            if (ModelState.IsValid)
            {
                Repository.InsertOrUpdate(field);
                
                await Repository.SaveAsync(LoggedUser);

                return RedirectToAction("FieldDetails","RootAdmin",new {fieldId = field.Id});
            }
            //TODO add model state errors if is required
           
            return View(field);
        }

        public async Task<ActionResult> FieldDetails(int fieldId)
        {
            return View(await Repository.FindByIdAsync(fieldId));
        }

        public ActionResult Fields_Read([DataSourceRequest] DataSourceRequest request)
        {
            var products = Repository.AsQueryable.ToDataSourceResult(request);
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Fields_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Field> fields)
        {
            var results = new List<Field>();

            if (fields != null && ModelState.IsValid)
            {
                Repository.InsertOrUpdate(fields);
                results.AddRange(fields);
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Fields_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Field> fields)
        {
            if (fields!= null && ModelState.IsValid)
            {
                Repository.InsertOrUpdate(fields);
            }

            return Json(fields.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Fields_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Field> fields)
        {
            if (fields.Any())
            {
                Repository.Delete(fields);
            }

            return Json(fields.ToDataSourceResult(request, ModelState));
        }
        #endregion

        #region Centers Actions
        public ActionResult Centers()
        {
            return View();

        }

        public async Task<ActionResult> CenterEdit(int id=0,int companyId=0)
        {
            ViewBag.CompanyId = companyId;
            var model = await CenterRepository.FindByIdAsync(id) ?? new Center();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CenterEdit(Center model)
        {
            if (ModelState.IsValid)
            {
                CenterRepository.InsertOrUpdate(model);
                
                await CenterRepository.SaveAsync(LoggedUser);

                return RedirectToAction("FieldDetails", "RootAdmin", new { id = model.Id });
            }
            //TODO add model state errors if is required
            return View(model);
        }

        public async Task<ActionResult> CenterDetails(int id)
        {
            return View(await CenterRepository.FindByIdAsync(id));
        }

        public ActionResult Centers_Read([DataSourceRequest] DataSourceRequest request)
        {
            var products = CenterRepository.AsQueryable.ToDataSourceResult(request);
            return Json(products, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Centers_Create([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Center> centers)
        {
            var results = new List<Center>();

            if (centers != null && ModelState.IsValid)
            {
                CenterRepository.InsertOrUpdate(centers);
                results.AddRange(centers);
            }

            return Json(results.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Centers_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Center> centers)
        {
            if (centers != null && ModelState.IsValid)
            {
                CenterRepository.InsertOrUpdate(centers);
            }

            return Json(centers.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Centers_Destroy([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<Center> centers)
        {
            if (centers.Any())
            {
                CenterRepository.Delete(centers);
            }

            return Json(centers.ToDataSourceResult(request, ModelState));
        }

        

        public async Task<ActionResult> CenterFields(int id)
        {
            var fields = await Repository.GetFieldsFromCenterAsync(id);

            return View(fields);
        }
        #endregion

        #region Conpanies

        public async Task<ActionResult> Companies()
        {
            var companies = await CompanyRepository.GetCompaniesAsync();

            return View(companies);
        }

        public async Task<ActionResult> CompanyEdit(int id=0 )
        {
            var company = await CompanyRepository.FindByIdAsync(id) ?? new Company();

            return View(company);
        }

        [HttpPost]
        public async Task<ActionResult> CompanyEdit(Company model)
        {

            if (ModelState.IsValid)
            {
                CompanyRepository.InsertOrUpdate(model);

                await CompanyRepository.SaveAsync(LoggedUser);

                return RedirectToAction("Companies");
            }
            return View(model);
        }


        public async Task<ActionResult> CompanyDetails(int id)
        {
            var company = await CompanyRepository.FindByIdAsync(id);

            return View(company);
        }

        public async Task<ActionResult> CompanyCenters(int id)
        {
            ViewBag.CompanyId = id;
            var companies = await CenterRepository.AsIQueryable().Where(c => c.Idcompany == id).ToListAsync();

            return View(companies);
        }

        [HttpPost]
        public async Task<ActionResult> CompanyCenterEditorForm(int id = 0,int companyId=0)
        {
            ViewBag.CompanyId = companyId;
            var center = await CenterRepository.FindByIdAsync(id) ?? new Center();

            return View(center);
        }

        [HttpPost]
        public async Task<ActionResult> CompanyCenterEdit(Center center)
        {
            if (ModelState.IsValid)
            {
                CenterRepository.InsertOrUpdate(center);

                await CenterRepository.SaveAsync(LoggedUser);

                return RedirectToAction("CompanyCenters", new {id = center.Idcompany});
            }
            return View(center);
        }

        public async Task<ActionResult> CompanyCenterDetails(int id, int companyId = 0)
        {
            ViewBag.CompanyId = companyId;
            var center = await CenterRepository.FindByIdAsync(id);

            return View(center);
        }

        public async Task<ActionResult> FieldsByCenter(int id)
        {
            var fields = await Repository.GetFieldsFromCenterAsync(id);

            return View(fields);
        }

        #endregion

        #region costs Actions

        public async Task<ActionResult> Costs(int fieldId )
        {
            return View(await CostRepository.GetByFieldIdAsync(fieldId));
        }

        public async Task<ActionResult> CostDetails(int id)
        {
            return View(await CostRepository.FindByIdAsync(id));
        }

        
        public async Task<ViewResult> CostEdit(int? id=null)
        {
            if(id.HasValue) return View(await CostRepository.FindByIdAsync(id.Value));

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CostEdit(Cost model)
        {
            if (ModelState.IsValid)
            {
                CostRepository.InsertOrUpdate(model);
                
                await CenterRepository.SaveAsync(LoggedUser);

                return RedirectToAction("CostDetails", "RootAdmin", new { id = model.Id });
            }
            //TODO add model state errors if is required
            return View(model);
        }


        #endregion


        public async Task<ActionResult> CenterAutoComplete(string query)
        {
            var Centers = await CenterRepository.SearchAsync (query);

            return Json(Centers.ToAutocomplete(), JsonRequestBehavior.AllowGet);
        }
    }
}