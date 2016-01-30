using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Helpers
{
    public  static class DataExtensions
    {

        public static T1 CopyFrom<T1, T2>(this T1 obj, T2 otherObject)
            where T1 : class
            where T2 : class
        {
            PropertyInfo[] srcFields = otherObject.GetType().GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);

            PropertyInfo[] destFields = obj.GetType().GetProperties(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);

            foreach (var property in srcFields)
            {
                var dest = destFields.FirstOrDefault(x => x.Name == property.Name);
                if (dest != null && dest.CanWrite)
                    dest.SetValue(obj, property.GetValue(otherObject, null), null);
            }

            return obj;
        }

        public static DateTime ToSpecificKind(this DateTime? dateTime ,DateTimeKind kind= DateTimeKind.Utc)
        {
            var date = DateTime.Now;

            return dateTime.HasValue ? 
                DateTime.SpecifyKind(dateTime.Value, kind)
                :DateTime.SpecifyKind(date, kind);


        }

        public static string Serialize<TModel>(this TModel model)
        {
            return JsonConvert.SerializeObject(model);
        }
    }
}
