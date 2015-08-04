using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access.Models;

namespace Access.Extensions
{
    public static class FieldExtensions
    {

        public static string ConfirmShoesType(this Field field,bool function )
        {
            return function ? "Si" : "No";
        }

        public static string DistanceFromMe(this Field field, DbGeography point)
        {
            return ( (field.Coordinates != null && point!=null )? 
                String.Format("{0:0.##} km", (field.Coordinates.Distance(point)) / 1000) : "-");
        }

        public static string DistanceFromMe(this Field field, string lat, string lon)
        {
            return DistanceFromMe(field, GetPointFrom(lat, lon));
        }

        public static DbGeography GetPointFrom(float lat, float lon)
        {
            var pointString = string.Format("POINT({0} {1})", lon, lat);


            return DbGeography.FromText(pointString, 4326);
        }
        public static DbGeography GetPointFrom(string lat, string lon)
        {
            if(string.IsNullOrEmpty(lat)||string.IsNullOrEmpty(lon))return null;
            var pointString = string.Format("POINT({0} {1})", lon, lat);


            return DbGeography.FromText(pointString, 4326);
        }

        
    }
}
