namespace EnLaCanchaAccess.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("COMPANY")]
    public partial class COMPANY
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IDENTRY { get; set; }

        [Required]
        [StringLength(15)]
        public string OBJECTTYPE { get; set; }

        [StringLength(100)]
        public string NAME { get; set; }

        [StringLength(20)]
        public string NIT { get; set; }

        [StringLength(20)]
        public string PHONE1 { get; set; }

        [StringLength(20)]
        public string PHONE2 { get; set; }

        [StringLength(200)]
        public string ADDRESS { get; set; }

        [StringLength(50)]
        public string PERSONCONTACT { get; set; }

        public int? USERSIGN { get; set; }

        public DateTime? CREATEDATE { get; set; }

        public int? CREATETIME { get; set; }

        [StringLength(1)]
        public string STATUS { get; set; }
    }
}
