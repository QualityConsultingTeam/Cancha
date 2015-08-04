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
    public class Team :BaseModel
    {

        public Team()
        {
            CreatedDate = DateTime.Now;
        }

        [DisplayName("Nombre")]
        public  string Name{get;set;}

        [DisplayName("Comentarios")]
        public string Comment { get; set; }


        [DisplayName("Fecha Creacion")]
        [Required]
        public DateTime CreatedDate { get; set; }

        // Aspnet Identity Id
        public List<Guid> TeamMembers { get; set; }
    }
}
