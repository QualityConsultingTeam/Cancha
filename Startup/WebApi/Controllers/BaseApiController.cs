using Access;
using Access.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace WebApi.Controllers
{
    //[Globalization]
    //[RequireHttps]
    public class BaseApiController<TRepository, TContext, TEntity> : ApiController //new()
        where TContext : DbContext
        where TEntity : BaseModel
        where TRepository : BaseRepository<TContext, TEntity>, new()
    {




        protected TContext Context
        {
            get
            {

                return Request.GetOwinContext().Get<TContext>();
            }
        }

        protected TRepository Repository
        {
            get
            {
                return new TRepository()
                {
                    Context = Context
                };
            }
        }

        public Guid? LoggedUser
        {
            get
            {
                if (!User.Identity.IsAuthenticated) return null;

                var id = User.Identity.GetUserId();

                return new Guid(id);
            }
        }

        protected IHttpActionResult ResultAction<Tmodel>(Tmodel model) where Tmodel: BaseModel
        {
            if (model == null) return NotFound();
            return Ok(model);
        }
    }
}
