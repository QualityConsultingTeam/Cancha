using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models
{
    public  class TeamJoinRequest: BaseModel
    {

        public TeamJoinRequest()
        {
            CreatedDate = DateTime.Now;
        }

        public Guid Requester { get; set; } // User who validate a new member request

        public Guid RequestTo { get; set; } // user that wan t o join in team

        public bool IsValid { get; set; }

        public bool IsDennied { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
