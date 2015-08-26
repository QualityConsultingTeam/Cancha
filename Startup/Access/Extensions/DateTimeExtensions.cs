using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access
{
    public static  class DateTimeExtensions
    {

        public static DateTime ParseFromString(string date)
        {
            if (date.Contains("GMT"))
            {
                date = (date.Trim().Replace("GMT","*").Split('*').FirstOrDefault()??"" ).Trim();

            }
            DateTime result;

            return (DateTime.TryParse(date, out result)) ? result : DateTime.Now ;
        }

       
    }
}
