using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models
{

    public enum Lawntypes
    {
        Sintetica = 0,
        Grama = 1,
        Semento = 2,
        Tierra = 3,
        Otro =4,
    }

    public enum FieldType
    {
        FUT =0,
        FUT5 =1,
        FUT7 =2,
        FUT9 =3,
        FUT11 = 4,
        TENNIS = 5,
        ETC = 6,
    }

    public enum ShoesTypes
    {
        Tenis=0,
        Tacos =1,
    }

    public enum Status
    {
        //Socatus!!!
        Pendiente =0,
        Inactivo = 1,
        Activo =2, 
    }

    public enum BookingStatus
    {
        Pendiente=0,
        Reservada =1,
        Finalizado =2,
        Denegado =3,
        Cancelado =4,
        Falta =5,
        
    }

    public enum BookingType
    {
        Manual =0,
        Automatico =1,
    }

    public enum ActionTypes
    {
        NotificacionSiple =0,
        Reclamo =1,

    }

}
