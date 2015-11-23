using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models
{
    public class Category : BaseModel
    {
        public string Name { get; set; }

        public List<Feed> Feeds { get; set; }

    }
}
