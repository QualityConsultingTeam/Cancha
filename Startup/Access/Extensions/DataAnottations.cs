using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Extensions
{
    public class CustomDateTimeDisplay : DisplayFormatAttribute
    {

        public CustomDateTimeDisplay()
        {
            this.DataFormatString = ModelConstants.DateDataAnnotationFormat;
            this.ApplyFormatInEditMode = true;
        }
    }


    
}
