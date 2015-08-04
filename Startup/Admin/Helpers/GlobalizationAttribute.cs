using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Admin
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class Globalization : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            GlobalizationStart Globalization = new GlobalizationStart();
        }
        private class GlobalizationStart
        {
            public GlobalizationStart()
            {
                // Hardcoded
                string culture = "es-SV";

                
                var cultureInfo = new CultureInfo(culture ?? FromBrowser());
                Thread.CurrentThread.CurrentCulture = cultureInfo;
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                
            }
            private string FromBrowser()
            {
                if (HttpContext.Current == null || HttpContext.Current.Request == null ||
                    HttpContext.Current.Request.UserLanguages == null) return "es";

                var lang = HttpContext.Current.Request.UserLanguages.FirstOrDefault();

                return !string.IsNullOrEmpty(lang) ? lang : "es-SV";
            }
          

        }



    }
}
