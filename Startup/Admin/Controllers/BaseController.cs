using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Access;
using Access.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
namespace Admin.Controllers
{
    //[Globalization]
    [RequireHttps]
    public class BaseController<TRepository, TContext, TEntity> : Controller //new()
        where TContext : DbContext 
        where TEntity :  BaseModel 
        where TRepository : BaseRepository<TContext,TEntity>, new()
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
    }
}