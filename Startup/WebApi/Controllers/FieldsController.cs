using Access;
using Access.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApi.Controllers
{
    public class FieldsController : BaseApiController<FieldsRepository,AccessContext,Field>
    {

        [HttpGet]
        public async Task< IHttpActionResult> Fields(string keywords="" ,string lat ="", string lon="")
        {
            var filter = new FilterOptionModel() { keywords = keywords, lat = lat, lon = lon };

            var model = await Repository.FullSearchAsync(filter);

            if (model == null) return NotFound();

            return Ok(model);
         }
    }
}
