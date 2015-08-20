﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access.Models;
using Kendo.Mvc.UI;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Admin.Models
{
    public class BookingViewModel : ISchedulerEvent 
    {

        [DisplayName("Titulo")]
        public string Title { get; set; }

        public string FieldSearchName { get; set; }

        [DisplayName("Descripcion")]
        public string Description { get; set; }

        [DisplayName("Dia Completo")]
        public bool IsAllDay { get; set; }

        [DisplayName("Inicio")]
        public DateTime Start { get; set; }

        [DisplayName("Final")]
        public DateTime End { get; set; }

        [DisplayName("Cancha")]
        public int? Idcancha { get; set; }

        public string StartTimezone { get; set; }
        public string EndTimezone { get; set; }
        public string RecurrenceRule { get; set; }
        public string RecurrenceException { get; set; }

        public int RecurrenceId { get; set; }
        public int Id { get; internal set; }

        [DisplayName("Cliente")]
        public Guid Userid { get; internal set; }

        public UserInfo UserInfo { get; set; }

        [DataType(DataType.Currency)]
        [DisplayName("Precio Promo")]
        public decimal ? CustomPrice { get; set; }

        [DisplayName("Descuento %")]
        public decimal ? Off { get; set; }


        public bool HasPromo
        {
            get { return CustomPrice.HasValue || Off.HasValue; }
        }
    }
}
