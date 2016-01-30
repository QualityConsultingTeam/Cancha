using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using Access.Extensions;
using System.Security.Claims;

namespace Access.Models
{
    public class FilterOptionModel
    {

        public FilterOptionModel()
        {
            page = 1;
            Limit = 10;
            //centerid = ClaimsPrincipal.Current.CenterId();

        }
        public FilterOptionModel(string centerIdClaim )
        {
            page = 1;
            Limit = 10;
            //centerid = !string.IsNullOrEmpty(centerIdClaim) ? Convert.ToInt32(centerIdClaim) : 0;
        }
        
        public string keywords { get; set; }

        public DateTime? date { get; set; }

        public string time { get; set; }

        public DateTime? end { get; set; }

        public string lat { get; set; }

        public string lon { get; set; }


        public int page { get; set; }

        public int Limit { get; set; }

        public bool IsDesc { get; set; }

        public BookingStatus? BookingStatus { get; set; }

        public string OrderByProperty { get; set; }


        public string role { get; set; }

        //public int? centerid { get; set; }

        public int Skip
        {
            get { return page == 1 || page == 0 ? 0 : (page - 1) * Limit; }
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


        public string CategoryName { get; set; }
        public FeedStatus? FeedStatus { get; internal set; }
    }
}
