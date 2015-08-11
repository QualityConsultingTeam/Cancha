using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace Admin
{
    public static class HMTLHelperExtensions
    {
        public static string IsSelected(this HtmlHelper html, string controller = null, string action = null)
        {
            string cssClass = "active";
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (String.IsNullOrEmpty(controller))
                controller = currentController;

            if (String.IsNullOrEmpty(action))
                action = currentAction;

            return controller == currentController && action == currentAction ?
                cssClass : String.Empty;
        }

        public static string PageClass(this HtmlHelper html)
        {
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            return currentAction;
        }

        public static MvcHtmlString TypeaheadFor<TModel, TValue>(
     this HtmlHelper<TModel> htmlHelper,
     Expression<Func<TModel, TValue>> expression,
     IEnumerable<string> source,
     int items = 8)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            if (source == null)
                throw new ArgumentNullException("source");

            var jsonString = new JavaScriptSerializer().Serialize(source);

            return htmlHelper.TextBoxFor(
                expression,
                new
                {
                    autocomplete = "off",
                    data_provide = "typeahead",
                    data_items = items,
                    data_source = jsonString
                }
            );
        }
           public static MvcHtmlString TypeaheadForAsync<TModel, TValue>(
     this HtmlHelper<TModel> htmlHelper,
     Expression<Func<TModel, TValue>> expression,
     string actionSource, string controller,
     object htmlAttributes =null,
     int items = 8)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            var dictAttributes =htmlAttributes!=null ? 
                htmlAttributes.ToDictionary():
                 (new
                 {
                     autocomplete = "off",
                     data_provide = "typeahead",
                 }).ToDictionary();

            var result = new ExpandoObject();
            var d = result as IDictionary<string, object>; //work with the Expando as a Dictionary

            d.Add("autocomplete","off");
            d.Add("data_provide", "typeahead");

            if (dictAttributes != null)
            {
                foreach (var pair in dictAttributes)
                {
                    d[pair.Key] = pair.Value;
                }
            }

               var searchbox = htmlHelper.TextBoxFor(expression, result);
            var textbox = htmlHelper.HiddenFor(expression);

             var idName = typeof (TValue).Name;
             var script=  htmlHelper.Partial("TypeAhead.cshtml", new TypeAheadParameter
               {
                   Id =string.Format("{0}_search", idName),
                   ActionSource = actionSource,
                   Controller = controller
               });
               return MvcHtmlString.Create(textbox.ToString()+script.ToString());
        }
           public static IDictionary<string, object> ToDictionary(this object data)
           {
               if (data == null) return null; // Or throw an ArgumentNullException if you want
                

               BindingFlags publicAttributes = BindingFlags.Public | BindingFlags.Instance;
               Dictionary<string, object> dictionary = new Dictionary<string, object>();

               foreach (PropertyInfo property in
                        data.GetType().GetProperties(publicAttributes))
               {
                   if (property.CanRead)
                   {
                       dictionary.Add(property.Name, property.GetValue(data, null));
                   }
               }
               return dictionary;
           }
           public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression)
           {
               ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
               IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

               IEnumerable<SelectListItem> items =
                   values.Select(value => new SelectListItem
                   {
                       Text = value.ToString(),
                       Value = value.ToString(),
                       Selected = value.Equals(metadata.Model)
                   });

               return htmlHelper.DropDownListFor(
                   expression,
                   items
                   );
           }

           public static IHtmlString ImageActionLink(this AjaxHelper helper, string imageUrl, string altText, string actionName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes = null)
           {
               var builder = new TagBuilder("img");
               builder.MergeAttribute("src", imageUrl);
               builder.MergeAttribute("alt", altText);
               builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
               var link = helper.ActionLink("[replaceme]", actionName, routeValues, ajaxOptions).ToHtmlString();
               return MvcHtmlString.Create(link.Replace("[replaceme]", builder.ToString(TagRenderMode.SelfClosing)));
           }
        //        $("#Search").typeahead({
        //    source: function (query, process) {
        //        var countries = [];
        //        map = {};

        //        // This is going to make an HTTP post request to the controller
        //        return $.post('/Client/CountryLookup', { query: query }, function (data) {

        //            // Loop through and push to the array
        //            $.each(data, function (i, country) {
        //                map[country.Name] = country;
        //                countries.push(country.Name);
        //            });

        //            // Process the details
        //            process(countries);
        //        });
        //    },
        //    updater: function (item) {
        //        var selectedShortCode = map[item].ShortCode;

        //        // Set the text to our selected id
        //        $("#details").text("Selected : " + selectedShortCode);
        //        return item;
        //    }
        //});

        public static void Assign(this object destination, object source)
        {
            if (destination is IEnumerable && source is IEnumerable)
            {
                var dest_enumerator = (destination as IEnumerable).GetEnumerator();
                var src_enumerator = (source as IEnumerable).GetEnumerator();
                while (dest_enumerator.MoveNext() && src_enumerator.MoveNext())
                    dest_enumerator.Current.Assign(src_enumerator.Current);
            }
            else
            {
                var destProperties = destination.GetType().GetProperties();
                foreach (var sourceProperty in source.GetType().GetProperties())
                {
                    foreach (var destProperty in destProperties)
                    {
                        if (destProperty.Name == sourceProperty.Name &&
                            destProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType) && destProperty.CanWrite)
                        {
                            destProperty.SetValue(destination, sourceProperty.GetValue(source, new object[] { }),
                                new object[] { });
                            break;
                        }
                    }
                }
            }
        }
    }
}
