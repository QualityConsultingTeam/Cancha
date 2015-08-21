using Access.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Admin.Models
{
    public static class AutocompleteHelpers
    {
        public static MvcHtmlString AutocompleteFor<TModel, TProperty1, TProperty2>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty1>> valueExpression,
            Expression<Func<TModel, TProperty2>> idExpression, string actionName, string controllerName, bool requestFocus)
        {
            string autocompleteUrl = UrlHelper.GenerateUrl(null, actionName, controllerName,
                                                           null,
                                                           html.RouteCollection,
                                                           html.ViewContext.RequestContext,
                                                           includeImplicitMvcValues: true);
            string @class = "form-control typeahead" + (requestFocus ? " focus" : string.Empty);
            // Get the fully qualified class name of the autocomplete id field
            string idFieldString = idExpression.Body.ToString();
            // We need to strip the 'model.' from the beginning
            int loc = idFieldString.IndexOf('.');
            // Also, replace the . with _ as this is done by MVC so the field name is js friendly
            string autocompleteIdField = idFieldString.Substring(loc + 1, idFieldString.Length - loc - 1).Replace('.', '_');
            return html.TextBoxFor(valueExpression, new { data_autocomplete_url = autocompleteUrl, @class, data_autocomplete_id_field = autocompleteIdField });
        }

        public static MvcHtmlString AutocompleteFor<TModel, TProperty1, TProperty2>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty1>> valueExpression,
           Expression<Func<TModel, TProperty2>> idExpression, string actionName, string controllerName, bool requestFocus,string customcss)
        {
            string autocompleteUrl = UrlHelper.GenerateUrl(null, actionName, controllerName,
                                                           null,
                                                           html.RouteCollection,
                                                           html.ViewContext.RequestContext,
                                                           includeImplicitMvcValues: true);
            string @class = customcss + (requestFocus ? " focus" : string.Empty);
            // Get the fully qualified class name of the autocomplete id field
            string idFieldString = idExpression.Body.ToString();
            // We need to strip the 'model.' from the beginning
            int loc = idFieldString.IndexOf('.');
            // Also, replace the . with _ as this is done by MVC so the field name is js friendly
            string autocompleteIdField = idFieldString.Substring(loc + 1, idFieldString.Length - loc - 1).Replace('.', '_');
            return html.TextBoxFor(valueExpression, new { data_autocomplete_url = autocompleteUrl, @class, data_autocomplete_id_field = autocompleteIdField });
        }


        public static List<AutoCompleteModel> ToAutocomplete(this List<ApplicationUser> users)
        {
            return users.ToIdentityUserViewModel()
                .Select(i => new AutoCompleteModel()
                {
                    Id = i.Id,
                    Name = string.Format("{0}  {1}", i.FirstName, i.LastName),
                }).ToList();
        }

        public static List<Autocomplete> ToAutocomplete(this List<Center> model)
        {
            return model.Select(c => new Autocomplete()
            {
                Id = c.Id,
                Name = String.Format("{0} - {1}", c.Name.ToUpper(), c.Email)
            }).ToList();
        }

        public static List<Autocomplete> ToAutocomplete(this List<Field> model)
        {
            return model.Select(c => new Autocomplete()
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }
    }
}
