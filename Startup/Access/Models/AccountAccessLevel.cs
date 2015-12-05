using Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models
{
    public class AccountAccessLevel : BaseModel
    {

        public List<int> CentersKeys { get; set; }


        public Center Center { get; set; }

        public int CenterId { get; set; }

        public string  UserId { get; set; }

        public ApplicationUser IdentityUser { get; set; }

        public bool Locked { get; set; }


    }
}
