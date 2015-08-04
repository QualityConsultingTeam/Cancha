using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models
{
    public class ObjectTypes
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(15)]
        public string OBJECTTYPE { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(3)]
        public string CODE { get; set; }

        [StringLength(100)]
        public string NAME { get; set; }

        [StringLength(250)]
        public string DESCRIPTION { get; set; }

        [StringLength(1)]
        public string STATUS { get; set; }
    }
}
