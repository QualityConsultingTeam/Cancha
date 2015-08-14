﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Access.Extensions;
using Access.Models;
 

namespace Access.Repositories
{
    public class BookingRepository: BaseRepository<AccessContext,Booking>
    {

 

        public IEnumerable<BookingStatus> Statuses
        {
            get
            {
                return  Enum.GetValues(typeof(BookingStatus)).Cast<BookingStatus>();

            }
        }

        #region Common
        public IQueryable<Booking> GetSummary(Guid userid )
        {

            // filtrar por nievel de acceso 
            var fieldsIds = Context.Centers.Include("Fields")
                .Where(c =>  c.Employees.Any(employee => employee.AccountId == userid))
                .SelectMany(c => c.Fields.Select(f => f.Id)).ToList();
        
            // agregar nivel de acceso para visibilidad
                          
            IQueryable<Booking> query =Context.Bookings.Include("Field");
            
            if (fieldsIds.Any()) query = query.Where(b => b.Start.HasValue &&
                                                          b.End.HasValue && 
                                                          b.Userid != Guid.Empty &&
                                                          fieldsIds.Contains(b.Field.Id));

            return query;
        }
        private IQueryable<Booking> CommonSearch(FilterOptionModel filter,Guid user)
        {
            IQueryable<Booking> query = GetSummary(user);

            filter.SearchKeys.ForEach(k => query = query.Where(q => q.Field.Name.ToLower().Contains(k)));
             

            if (filter.date.HasValue) query = query.Where(q => q.Start >= filter.date.Value);

            if (filter.end.HasValue) query = query.Where(q => q.End <= filter.end.Value);

            if (filter.BookingStatus.HasValue) query = query.Where(b => b.Status == filter.BookingStatus.Value);

            return filter.HasOrderByProperty ? query.CustomOrderby(filter) : query.OrderBy(o => o.Start);
        }
        #endregion

        public Task<List<Booking>> GetSummaryAsync(Guid userid, int skip=0, int take=10)
        {
            return GetSummary(userid).OrderBy(c => c.CreateDate).Skip(skip).Take(take).ToListAsync();
        }

      

        public Task<List<Booking>> GetSummaryAsync(FilterOptionModel filter,Guid user)
        {
            return CommonSearch(filter, user).Skip(filter.Skip).Take(filter.Limit).ToListAsync();
        }

        public async Task <int> GetPageLimit(FilterOptionModel filter,Guid user)
        {
            return (await CommonSearch(filter, user).CountAsync() )/ filter.Limit +1;
        }

        public Task<Field> GetFieldForModel(Booking booking)
        {
            return Context.Fields.FirstOrDefaultAsync(c => c.Id == booking.Id);
        }


        public Task<List<UserInfo>> GetLastestAccountInfo(List<Booking> data)
        {

            return null; 
        }


        public string MessageForStatus(BookingStatus status)
        {
            switch (status)
            {
                case BookingStatus.Pendiente: return "";
                case BookingStatus.Reservada: return "Desea Confirmar Reserva";
                case BookingStatus.Finalizado: return "Se han Efectuado los pagos Correspondientes?";
                case BookingStatus.Denegado: return "Desea denegar la solicitud de Reserva";
                case BookingStatus.Cancelado: return "Desea Cambiar el estado a Cancelado ";
                case BookingStatus.Falta: return "El Usuario falto a la Solicitud de Reserva";
                default: return string.Empty;
            }
            
        }
       

       
    }
}
