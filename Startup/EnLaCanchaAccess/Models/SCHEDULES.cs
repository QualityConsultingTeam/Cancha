namespace EnLaCanchaAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SCHEDULES
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

        public int? DAYOFWEEK { get; set; }

        public int? IDCENTER { get; set; }

        public bool? ISOPEN { get; set; }

        public int? USERSIGN { get; set; }

        public DateTime? CREATEDATE { get; set; }

        public int? CREATETIME { get; set; }
    }
}
