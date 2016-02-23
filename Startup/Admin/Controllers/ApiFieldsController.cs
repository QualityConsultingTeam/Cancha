using Access;
using Access.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Admin.Controllers
{
    public class ApiFieldsController : BaseApiController<FieldsRepository, AccessContext, Field>
    {

        [HttpGet]
        [Route("api/canchas")]

        public async Task<IHttpActionResult> Fields(string keywords = "", string lat = "", string lon = "", DateTime? date = null)
        {
            var filter = new FilterOptionModel() { keywords = keywords, lat = lat, lon = lon, date = date };

            var model = await Repository.FullSearchAsync(filter);

            if (model == null) return NotFound();

            return Ok(model);
        }

        [HttpPost]
        [Route("api/canchas")]
        public async Task<IHttpActionResult> Fields(FilterOptionModel filter)
        {

            var model = await Repository.FullSearchAsync(filter);

            if (model == null) return NotFound();

            return Ok(model);
        }
    }
}
