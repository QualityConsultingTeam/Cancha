namespace EnLaCanchaAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SERVICES
    {
        [Key]
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IDENTRY { get; set; }

        [Required]
        [StringLength(15)]
        public string OBJECTTYPE { get; set; }

        [StringLength(100)]
        public string NAME { get; set; }

        public int? TYPE { get; set; }

        [StringLength(250)]
        public string DESCRIPTION { get; set; }

        [StringLength(1)]
        public string STATUS { get; set; }
    }
}
