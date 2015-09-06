using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models
{
    public class UserCenter : BaseModel
    {

        public UserCenter()
        {
           
        }

        public Guid UserId { get; set; }

        public int CenterId { get; set; }

        public Center Center { get; set; }


        public bool Locked { get; set; }
    }
}
