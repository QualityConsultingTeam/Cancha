namespace EnLaCanchaAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PAYMENT")]
    public partial class PAYMENT
    {
        [Key]
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IDENTRY { get; set; }

        [Required]
        [StringLength(15)]
        public string OBJECTTYPE { get; set; }

        public DateTime? DOCDATE { get; set; }

        public int? DOCTIME { get; set; }

        public int? USERID { get; set; }

        [StringLength(250)]
        public string DESCRIPTION { get; set; }

        [StringLength(250)]
        public string COMMENTS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? AMOUNT { get; set; }

        public int? USERSIGN { get; set; }

        public DateTime? CREATEDATE { get; set; }

        public int? CREATETIME { get; set; }
    }
}
