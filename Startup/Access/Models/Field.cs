using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
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

        [DataType(DataType.ImageUrl)]
        public string FieldPicure { get; set; }

        [DisplayName("Nombre")]
        [StringLength(100)]
        public string Name { get; set; }

        [DisplayName("Centro")]
        public int? CenterId { get; set; }

        [DisplayName("Tipo de Cancha")]
        public FieldType Type { get; set; }

        [DisplayName("Ubicacion")]
        [StringLength(200)]
        public string Location { get; set; }


        [ScriptIgnore]
        [DisplayName("Coordenadas")]
        public DbGeography Coordinates { get; set; }
        
        [NotMapped]
        [DisplayName("Distancia")]
        public string Distance { get; set; }

        [DisplayName("Descripcion")]
        [StringLength(250)]
        public string Description { get; set; }

        [DisplayName("Comentarios")]
        [StringLength(250)]
        public string Comments { get; set; }

        [DisplayName("Cuidad")]
        public int? Town { get; set; }

        [DisplayName("Departamento")]
        public int? Department { get; set; }

        [DisplayName("Pais")]
        public int? Country { get; set; }

        [DisplayName("Cuidad / Barrio")]
        [StringLength(100)]
        public string Neighborhood { get; set; }

        
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

        //[DisplayName("Usuario")]
        //// user thats create or update object in context
        //public int? UserSign { get; set; }

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

        public Cost Cost { get; set; }
        //public int CostId { get; set; }

       
        public Center Center { get; set; }
       
    }
}
