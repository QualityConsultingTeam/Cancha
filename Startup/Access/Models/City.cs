using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models
{
    public class City:BaseModel
    {
        public City()
        {
            CityIp = new HashSet<CityIp>();
            //Place = new HashSet<Place>();
            //Zone = new HashSet<Zone>();
        }

        public City(int id, string name , int stateid)
        {
            this.Id = id;
            CityName = name;
            this.StateId = stateid;
        }
        [DisplayName("Nombre")]
        [StringLength(50)]
        public string CityName { get; set; }

        [DisplayName("Estado")]
        public int StateId { get; set; }

        public virtual States States { get; set; }

        public virtual ICollection<CityIp> CityIp { get; set; }

        //public virtual ICollection<Place> Place { get; set; }

        //public virtual ICollection<Zone> Zone { get; set; }
    }
}
