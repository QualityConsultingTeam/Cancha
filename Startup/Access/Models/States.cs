using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models
{
    public class States :BaseModel
    {
        public States()
        {
            Cities = new HashSet<City>();
        }

        public States(int id, string name, int country)
        {
            this.Id = id;
            this.State = name;
            this.IdCountry = country;

        }

        public int IdCountry { get; set; }

        [StringLength(50)]
        public string State{ get; set; }

        public virtual ICollection<City> Cities { get; set; }

        public virtual Country Country { get; set; }
    }
}
