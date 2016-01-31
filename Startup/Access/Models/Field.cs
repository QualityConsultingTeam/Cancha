using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web.Script.Serialization;

namespace Access.Models
{
    public class Field : TrackedEntity
    {

        public Field()
        {

            CreateDate = DateTime.Now;
            CreateTime = DateTime.Now.Hour;
        }

        [DisplayName("Nombre")]
        [StringLength(100)]
        public string Name { get; set; }

        [DisplayName("Centro")]
        public int? CenterId { get; set; }

        [DisplayName("Tipo de Cancha")]
        public FieldType Type { get; set; }

        [NotMapped]
        [DisplayName("Distancia")]
        public string Distance { get; set; }

        [DisplayName("Descripcion")]
        [StringLength(250)]
        public string Description { get; set; }

        [DisplayName("Comentarios")]
        [StringLength(250)]
        public string Comments { get; set; }

        [DisplayName("Longitud")]
        [Column(TypeName = "numeric")]
        public decimal? Length { get; set; }

        [DisplayName("Ancho")]
        [Column(TypeName = "numeric")]
        public decimal? Width { get; set; }

        [DisplayName("Tipo de Grama")]
        public Lawntypes Lawn { get; set; }

        [DisplayName("Tipo de zapatos")]
        public ShoesTypes Shoes { get; set; }

        [DisplayName("Calificacion")]
        // given by user from 1 to 5
        [Range(0, 5, ErrorMessage = "El valor no puede ser mayor que 5")]
        [Column(TypeName = "numeric")]
        public decimal? Grade { get; set; }

        [DisplayName("Estado")]
        public Status Status { get; set; }

        [DisplayName("Fecha Creacion")]
        [Required]
        public DateTime CreateDate { get; set; }

        [DisplayName("Hora Creacion")]

        public int? CreateTime { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Required]

        public int OBJECTTYPE { get; set; }

        
        [ScriptIgnore]
        public List<Schedule> Shedules { get; set; }

        
        [ScriptIgnore]
        public List<Booking> Bookings { get; set; }

        public Center Center { get; set; }

        public List<Cost> Costs { get; set; }

        

    }
}
