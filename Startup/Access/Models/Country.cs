using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models
{
    public class Country :BaseModel
    {
        public Country() { }
        public Country(int id, string name , string Iso2,string iso3,string phonecode,bool? isactive)
        {
            this.Id = id;
            this.Name = name;
            this.Iso2 = Iso2;
            this.Iso3 = iso3;
            this.Phone_code = phonecode;
            this.Is_active = isactive;

        }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Iso2 { get; set; }

        [StringLength(50)]
        public string Iso3 { get; set; }

        [StringLength(50)]
        public string Phone_code { get; set; }

        public bool? Is_active { get; set; }

        public virtual ICollection<States> States { get; set; }
    }
}
