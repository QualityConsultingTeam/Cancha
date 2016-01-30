using System;
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
        public async Task< IQueryable<Booking> >GetSummary(Guid? userid = null, bool onlyAvailables = false, List<string> usersids = null)
        {

            // agregar nivel de acceso para visibilidad

            IQueryable<Booking> query = Context.Bookings.Include(b => b.Field);

            if (usersids != null && usersids.Any()) query = query.Where(b => usersids.Contains(b.Userid));

            //// filtrar por nievel de acceso 
            var centerId = await Context.GetCenterIdAsync();// ClaimsPrincipal.Current.CenterId();

            if (centerId.HasValue)
            {
                query = (from booking in Context.Bookings
                         join field in Context.Fields.Where(f => f.CenterId == centerId)
                         on booking.Idcancha equals field.Id
                         select booking);

            }
            else Context.Bookings.Include(b => b.Field);

            if (onlyAvailables) query = query.Where(b => b.Status != BookingStatus.Denegado && b.Status != BookingStatus.Falta);


            query = query.Where(b => b.Start.HasValue &&
                                                          b.End.HasValue &&
                                                          !string.IsNullOrEmpty( b.Userid)
                                                          //!= Guid.Empty
                                                          );

            return query;
        }
        private async Task< IQueryable<Booking> >CommonSearch(FilterOptionModel filter, Guid user)
        {
            var _query = await GetSummary(user);
            IQueryable<Booking> query = _query.Include(b => b.Field).Include(b=>b.User);

            filter.SearchKeys.ForEach(k => query = query.Where(q => q.Field.Name.ToLower().Contains(k)));


            if (filter.date.HasValue) query = query.Where(q => q.Start >= filter.date.Value);

            if (filter.end.HasValue) query = query.Where(q => q.End <= filter.end.Value);

            if (filter.BookingStatus.HasValue) query = query.Where(b => b.Status == filter.BookingStatus.Value);

            //  if (filter.centerid.HasValue) query = query.Where(c => c.Field.CenterId == filter.centerid);

            return filter.HasOrderByProperty ? query.CustomOrderby(filter) : query.OrderByDescending(o => o.Start);
        }
        #endregion

        public async Task<IQueryable<BookingManageViewModel>> GetSummary(FilterOptionModel filter)
        {
            IQueryable<Booking> query = await CommonSearch(filter, UserId);

            return query.Select(b => new BookingManageViewModel()
            {
                Id = b.Id,
                UserEmail = b.User.Email,
                Userid = b.Userid,
                FieldName = b.Field.Name,
                FieldId= b.Idcancha,
                Start = b.Start,
                End =b.End,
                Type = b.Type,
                Status = b.Status,
                UserFullName = b.User.FirstName+" "+b.User.LastName,
            });
        }

        public async Task<List<Booking>> GetSummaryAsync(FilterOptionModel filter)
        {
            var query = await CommonSearch(filter, UserId);
            return await query.Skip(filter.Skip).Take(filter.Limit).ToListAsync();
        }

        public async Task <int> GetPageLimit(FilterOptionModel filter)
        {
            var query = await CommonSearch(filter, UserId);
            return await( query.CountAsync() )/ filter.Limit +1;
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

        public Task<List<Booking>> GetUserBookings(FilterOptionModel filter)
        {
            return Context.Bookings.Where(b => b.Userid == UserId.ToString()).Include(b=>b.Field)
                .OrderByDescending(b => b.CreateDate).Skip(filter.Skip).Take(filter.Limit).ToListAsync();
        }

        public async Task UpdateAccountLevel(Booking booking, bool ignoreStatus = false)
        {
            await booking.UpdateUserAccountLevel(Context);
        }

        
    }
}
