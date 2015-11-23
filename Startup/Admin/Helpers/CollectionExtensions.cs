using Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Admin.Helpers
{
    public static class CollectionExtensions
    {

        public static List<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, string> text, Func<T, string> value, string defaultOption)
        {
            var items = enumerable.Select(f => new SelectListItem() { Text = text(f), Value = value(f) }).ToList();
            items.Insert(0, new SelectListItem() { Text = defaultOption, Value = "-1" });
            return items;
        }


        public static List<Autocomplete> ToAutoComplete<T>(this IEnumerable<T> enumerable, Func<T, int> Id, Func<T, string> Name)
        {
            return enumerable.Select(f => new Autocomplete() { Id = Id(f), Name = Name(f) }).ToList();
        }




        public static List<Tuple<string, object>> GetProperties<TModel>(this TModel model)
        {
            var type = model.GetType();

            var result = type.GetProperties().ToList().Select(p =>
            {
                PropertyInfo prop = type.GetProperty(p.Name);

                return Tuple.Create(p.Name, prop.GetValue(model));
            }).ToList();

            return result;
        }
    }
}
