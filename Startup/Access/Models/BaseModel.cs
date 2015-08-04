using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models
{
    public class BaseModel
    {
        [Key]
        public int Id { get; set; }
    }

    public class TrackedEntity :BaseModel
    {

        public TrackedEntity()
        {
            
        }
         
        [DisplayName("Usuario")]
        public virtual Guid UserSign { get; set; }

        
    }
}
