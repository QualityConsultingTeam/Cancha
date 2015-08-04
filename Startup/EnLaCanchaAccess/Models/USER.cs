namespace EnLaCanchaAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("USER")]
    public partial class USER
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

        [Required]
        [StringLength(15)]
        public string NICKNAME { get; set; }

        [Required]
        [StringLength(150)]
        public string PASSWORD { get; set; }

        [StringLength(25)]
        public string DUI { get; set; }

        [StringLength(20)]
        public string PHONE_1 { get; set; }

        [StringLength(20)]
        public string PHONE_2 { get; set; }

        [StringLength(50)]
        public string EMAIL { get; set; }

        [StringLength(200)]
        public string ADDRESS { get; set; }

        [StringLength(100)]
        public string COORDINATES { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? CATEGORY { get; set; }

        public int? IDCENTER { get; set; }

        [StringLength(1)]
        public string STATUS { get; set; }

        public int? USERSIGN { get; set; }

        public DateTime? CREATEDATE { get; set; }

        public int? CREATETIME { get; set; }
    }
}
