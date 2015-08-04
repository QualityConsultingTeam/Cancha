namespace EnLaCanchaAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("COST")]
    public partial class COST
    {
        [Key]
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IDENTRY { get; set; }

        [Required]
        [StringLength(15)]
        public string OBJECTTYPE { get; set; }

        public short? OPENTIME { get; set; }

        public short? CLOSETIME { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PRICE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DOWNPAYMENT { get; set; }

        public int? DAYOFWEEK { get; set; }

        public int? IDCANCHA { get; set; }

        public int? USERSIGN { get; set; }

        public DateTime? CREATEDATE { get; set; }

        public int? CREATETIME { get; set; }
    }
}
