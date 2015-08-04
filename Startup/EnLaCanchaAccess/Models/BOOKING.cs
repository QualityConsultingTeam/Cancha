namespace EnLaCanchaAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BOOKING")]
    public partial class BOOKING
    {
        [Key]
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IDENTRY { get; set; }

        [Required]
        [StringLength(15)]
        public string OBJECTTYPE { get; set; }

        public int? USERID { get; set; }

        public int? IDCANCHA { get; set; }

        public DateTime? START { get; set; }

        public DateTime? END { get; set; }

        [StringLength(25)]
        public string INVOICE { get; set; }

        [StringLength(1)]
        public string STATUS { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PRICE { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? DOWNPAYMENT { get; set; }

        public bool? HASDOWNPAY { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? PENDING { get; set; }

        [StringLength(50)]
        public string TEAM_1 { get; set; }

        [StringLength(50)]
        public string TEAM_2 { get; set; }

        public int? USERSIGN { get; set; }

        public DateTime? CREATEDATE { get; set; }

        public int? CREATETIME { get; set; }
    }
}
