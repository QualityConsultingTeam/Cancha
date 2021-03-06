﻿using Access.Extensions;
using Microsoft.Owin.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
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

        //public static string IsActive(this HtmlHelper html , expression)
        //{
        //    return expression. == true ? "active" : "";
        //}

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

        public static string ImageLink(this HtmlHelper htmlHelper, string imgSrc, string alt, string actionName, string controllerName, object routeValues, object htmlAttributes, object imgHtmlAttributes)
        {
            UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;
            TagBuilder imgTag = new TagBuilder("img");
            imgTag.MergeAttribute("src", imgSrc);
            imgTag.MergeAttributes((IDictionary<string, string>)imgHtmlAttributes, true);
            string url = urlHelper.Action(actionName, controllerName, routeValues);



            TagBuilder imglink = new TagBuilder("a");
            imglink.MergeAttribute("href", url);
            imglink.InnerHtml = imgTag.ToString();
            imglink.MergeAttributes((IDictionary<string, string>)htmlAttributes, true);

            return imglink.ToString();

        }
     

        public static string FacebookProfileSmallPicture(this HtmlHelper helper)
        {
            
            var claim =  ClaimsPrincipal.Current.FacebookProfileSmallPicture() ;

            return !string.IsNullOrEmpty(claim) ? claim : "~/Images/profile.jpg";
        }

        
    }
}
