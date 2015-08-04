using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models
{
    public class Challenge :BaseModel
    {

        public string Title { get; set; }
       
        public Challenge()
        {
            CreatedDate = DateTime.Now;
        }

       

        public DateTime CreatedDate { get; set; }

        public DateTime ChallengeDate { get; set; }


        public Team Team_1 { get; set; }

        public Team Team_2{ get; set; }

        

    }
}
