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

        [DisplayName("idCenter")]
        public int idCenter { get; set; }

        [DisplayName("imgUrl")]
        public String imgUrl { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public Center Center { get; set; }

        

    }
}
