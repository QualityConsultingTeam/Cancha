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
        public IQueryable<Booking> GetSummary(Guid ?userid =null,bool onlyAvailables=false, List<Guid> usersids= null)
        {

            // agregar nivel de acceso para visibilidad

            IQueryable<Booking> query = Context.Bookings.Include(b => b.Field);

            if (usersids != null && usersids.Any()) query = query.Where(b => usersids.Contains(b.Userid));

            //// filtrar por nievel de acceso 
            var centerId = ClaimsPrincipal.Current.CenterId();

            if (centerId.HasValue)
            {
                query = (from booking in Context.Bookings
                         join field in Context.Fields.Where(f => f.CenterId == centerId)
                         on booking.Idcancha equals field.Id
                         select booking);

            }
            else Context.Bookings.Include(b => b.Field);

            if (onlyAvailables) query = query.Where(b => b.Status != BookingStatus.Denegado && b.Status!= BookingStatus.Falta);


            query = query.Where(b => b.Start.HasValue &&
                                                          b.End.HasValue && 
                                                          b.Userid != Guid.Empty);

            return query;
        }
        private IQueryable<Booking> CommonSearch(FilterOptionModel filter,Guid user)
        {
            IQueryable<Booking> query = GetSummary(user).Include(b=> b.Field);
            
            filter.SearchKeys.ForEach(k => query = query.Where(q => q.Field.Name.ToLower().Contains(k)));
             

            if (filter.date.HasValue) query = query.Where(q => q.Start >= filter.date.Value);

            if (filter.end.HasValue) query = query.Where(q => q.End <= filter.end.Value);

            if (filter.BookingStatus.HasValue) query = query.Where(b => b.Status == filter.BookingStatus.Value);

           //  if (filter.centerid.HasValue) query = query.Where(c => c.Field.CenterId == filter.centerid);

            return filter.HasOrderByProperty ? query.CustomOrderby(filter) : query.OrderByDescending(o => o.Start);
        }
        #endregion

     
        public Task<List<Booking>> GetSummaryAsync(FilterOptionModel filter)
        {
            return CommonSearch(filter, UserId).Skip(filter.Skip).Take(filter.Limit).ToListAsync();
        }

        public async Task <int> GetPageLimit(FilterOptionModel filter)
        {
            return (await CommonSearch(filter, UserId).CountAsync() )/ filter.Limit +1;
        }

        public Task<Field> GetFieldForModel(Booking booking)
        {
            return Context.Fields.FirstOrDefaultAsync(c => c.Id == booking.Id);
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

        public async Task UpdateAccountLevel(Booking booking, bool ignoreStatus = false)
        {
            if (booking.Status == BookingStatus.Reservada || ignoreStatus)
            {
                var center = await Context.Centers.Where(c => c.Fields.Any(f => f.Id == booking.Idcancha))
                                                .FirstOrDefaultAsync();

                if (!await Context.AccountAccess.AnyAsync(c => c.UserId == booking.Userid))
                {
                    Context.AccountAccess.Add(new AccountAccessLevel()
                    {
                        UserId = booking.Userid,
                        Center = center,
                    });
                    await SaveAsync();
                }
            }
        }
    }
}
