namespace EnLaCanchaAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CENTER")]
    public partial class CENTER
    {
        [Key]
        [Column(TypeName = "numeric")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal IDENTRY { get; set; }

        [Required]
        [StringLength(15)]
        public string OBJECTTYPE { get; set; }

        public int? IDCOMPANY { get; set; }

        [StringLength(100)]
        public string NAME { get; set; }

        public int? TYPE { get; set; }

        [StringLength(200)]
        public string LOCATION { get; set; }

        [StringLength(100)]
        public string COORDINATES { get; set; }

        [StringLength(250)]
        public string DESCRIPTION { get; set; }

        public int? TOWN { get; set; }

        public int? DEPARTMENT { get; set; }

        public int? COUNTRY { get; set; }

        [StringLength(100)]
        public string NEIGHBORHOOD { get; set; }

        [StringLength(50)]
        public string ADMINISTRATOR { get; set; }

        [StringLength(50)]
        public string PERSONCONTACT { get; set; }

        [StringLength(20)]
        public string PHONE_1 { get; set; }

        [StringLength(20)]
        public string PHONE_2 { get; set; }

        [StringLength(50)]
        public string EMAIL { get; set; }

        public short? OPENTIME { get; set; }

        public short? CLOSETIME { get; set; }

        [StringLength(1)]
        public string STATUS { get; set; }

        public int? USERSIGN { get; set; }

        public DateTime? CREATEDATE { get; set; }

        public int? CREATETIME { get; set; }
    }
}
