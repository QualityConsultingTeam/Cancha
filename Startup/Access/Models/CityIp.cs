using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models
{
    public class CityIp:BaseModel
    {
        public double IpFrom { get; set; }

        public double IpTo { get; set; }

        public int CityID { get; set; }

        public double latitude { get; set; }

        public double longitude { get; set; }

        public virtual City City { get; set; }
    }
}
