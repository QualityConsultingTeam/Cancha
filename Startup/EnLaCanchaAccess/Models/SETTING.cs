namespace EnLaCanchaAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SETTING")]
    public partial class SETTING
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDENTRY { get; set; }

        [Required]
        [StringLength(15)]
        public string OBJECTTYPE { get; set; }

        [StringLength(10)]
        public string CODE { get; set; }

        public int? IDCOMPANY { get; set; }

        public int? TYPE { get; set; }

        [StringLength(250)]
        public string DESCRIPTION { get; set; }

        [StringLength(1)]
        public string STATUS { get; set; }

        public int? USERSIGN { get; set; }

        public DateTime? CREATEDATE { get; set; }

        public int? CREATETIME { get; set; }
    }
}
