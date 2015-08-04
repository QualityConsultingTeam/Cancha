namespace EnLaCanchaAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CANCHA")]
    public partial class CANCHA
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

        public int? IDCENTER { get; set; }

        public int? TYPE { get; set; }

        [StringLength(200)]
        public string LOCATION { get; set; }

        [StringLength(100)]
        public string COORDINATES { get; set; }

        [StringLength(250)]
        public string DESCRIPTION { get; set; }

        [StringLength(250)]
        public string COMMENTS { get; set; }

        public int? TOWN { get; set; }

        public int? DEPARTMENT { get; set; }

        public int? COUNTRY { get; set; }

        [StringLength(100)]
        public string NEIGHBORHOOD { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? LENGTH { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? WIDTH { get; set; }

        public int? LAWN { get; set; }

        public int? SHOES { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? GRADE { get; set; }

        [StringLength(1)]
        public string STATUS { get; set; }

        public int? USERSIGN { get; set; }

        public DateTime? CREATEDATE { get; set; }

        public int? CREATETIME { get; set; }
    }
}
