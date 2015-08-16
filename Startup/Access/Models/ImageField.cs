using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models
{
    public class ImageField : BaseModel
    {
        public ImageField() { }

        [DisplayName("IdCenter")]
        public int? IdCenter { get; set; }

        [DisplayName("imgUrl")]
        public String imgUrl { get; set; }

        [StringLength(50)]
        public String header1 { get; set; }

        [StringLength(50)]
        public String header2 { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Center Center { get; set; }

        

    }
}
