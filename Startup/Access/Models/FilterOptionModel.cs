using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Access.Extensions;

namespace Access.Models
{
    public class FilterOptionModel
    {

        public FilterOptionModel()
        {
            Page = 1;
            Limit = 10;

        }
        public string keywords { get; set; }

        public DateTime? date { get; set; }

        public string time { get; set; }

        public DateTime? end { get; set; }

        public string lat { get; set; }

        public string lon { get; set; }


        public int Page { get; set; }

        public int Limit { get; set; }

        public bool IsDesc { get; set; }

        public BookingStatus? BookingStatus { get; set; }

        public string OrderByProperty { get; set; }


        public int Skip
        {
            get { return Page == 1 || Page == 0 ? 0 : (Page - 1) * Limit; }
        }

        public bool HasOrderByProperty
        {
            get { return !string.IsNullOrEmpty(OrderByProperty); }
        }

        public List<string> SearchKeys
        {
            get
            {
                return (keywords ?? "").ToLower().Trim().Split(' ')
                    .Where(key => !string.IsNullOrEmpty(key)).ToList();
            }
        }
        

        public  DbGeography Point
        {
            get { return FieldExtensions.GetPointFrom(lat, lon); }
        }
    }
}
