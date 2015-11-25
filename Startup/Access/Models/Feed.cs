using Access.Extensions;
using Identity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Access.Models
{
    public class Feed : BaseModel
    {

        public Feed()
        {
            DateStart = DateTime.Now;
            DateEnd = DateTime.Now;
            LastUpdate = DateTime.Now;
        }

        [Display(Name = "Author")]
        public string Author { get; set; }


        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Content { get; set; }

        [Display(Name = "Status")]
        public FeedStatus Status { get; set; }

        [Display(Name = "Image")]
        [DataType(DataType.ImageUrl)]

        public string Image { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
       // [CustomDateTimeDisplay]

        public DateTime DateStart { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        //[CustomDateTimeDisplay]
        public DateTime DateEnd { get; set; }


        public Category Category { get; set; }

        public int CategoryId { get; set; }


        [Display(Name = "Publisher")]
        public string IdPublisher { get; set; }

        public ApplicationUser User { get; set; }


        public string AuthorName
        {
            get
            {
                return User != null ? $"{User.FirstName} {User.LastName}" : string.Empty;
                //return string.Empty;
            }
        }

        [Display(Name = "Option 1")]
        public string Option1 { get; set; }

        [Display(Name = "Option 2")]
        public string Option2 { get; set; }

        [Display(Name = "Option 3")]
        public string Option3 { get; set; }

        [Display(Name = "Option 4")]
        public string Option4 { get; set; }

        [Display(Name = "Option51")]
        public string Option5 { get; set; }
        public DateTime LastUpdate { get; internal set; }
    }
}
