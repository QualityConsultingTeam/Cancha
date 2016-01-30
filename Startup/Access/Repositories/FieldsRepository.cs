using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Spatial;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Access.Extensions;
using Access.Models;
using System.Security.Claims;

namespace Access
{
    public class FieldsRepository:BaseRepository<AccessContext,Field>
    {
        #region Comboboxes
        public async Task< IEnumerable<object>> GetCountriesAsync(string text)
        {
            var countries = !string.IsNullOrEmpty(text)
                ? Context.Countries.Where(c => c.Name.ToLower().Contains(text.ToLower()))
                : Context.Countries;
            return  await  countries.Select(c => new {id = c.Id, text = c.Name}).ToListAsync();
        }

        public async Task<IEnumerable<object>> GetStatesAsync(int? countryid, string keywords)
        {
            IQueryable<States> states = countryid.HasValue?
                Context.States.Where(s=> s.IdCountry== countryid.Value):
                Context.States;

            if (!string.IsNullOrEmpty(keywords))
            {
                keywords.ToLower().Split(' ').ToList().ForEach(keyword =>
                {
                    states = states.Where(st => st.State.ToLower().Contains(keyword));
                } );
            }

            return await states.Select(s => new {id = s.Id, text = s.State}).ToListAsync();
        }

        public async Task<Booking> FindWithPrice(Booking booking)
        {
            var field = await Context.Fields.Include(c => c.Center)
                        .FirstOrDefaultAsync(c => c.Id == booking.Idcancha.Value);
            
            var Starthour = booking.Start.Value.Hour;

            var cost = await Context.Costs
                .Where(c => c.IdCancha== booking.Idcancha.Value &&
                Starthour >= c.Opentime  && Starthour <= c.Closetime)
                .OrderByDescending(c => c.Price)
                .Select(c => c.Price).FirstOrDefaultAsync();

            booking.Price = cost ?? 18;
            //Context.Entry(field)
            //    .Collection(f => f.Costs)
            //    .Query()
            //    .Where(c => c.Opentime >= Starthour && Starthour <= c.Closetime).Load();
            booking.Field = field;
            return booking;

        }

        

        public async Task<IEnumerable<object>> GetCitiesAsync(int? stateId, string keywords)
        {
            IQueryable<City> cities = stateId.HasValue ?
               Context.Cities.Where(s => s.StateId == stateId.Value) :
               Context.Cities;

            if (!string.IsNullOrEmpty(keywords))
            {
                keywords.ToLower().Split(' ').ToList()
               .ForEach(keyword =>cities = cities.Where(st => st.CityName.ToLower().Contains(keyword)));
            }

            return await cities.Select(s => new { id = s.Id, text = s.CityName }).ToListAsync();
        }

        #endregion

        #region Common Search
        public IQueryable<Field> Search(FilterOptionModel filter)
        {

            IQueryable<Field> query = Search(filter.keywords ?? "");

            query = filter.Point != null
                ? query.OrderBy(f => f.Center.Coordinates.Distance(filter.Point))
                : query.OrderBy(f => f.Name);

            return query;
        }

        private IQueryable<Field> Search(string keywords)
        {
            var today = DateTime.Now.Date;
            IQueryable<Field> query = Context.Fields
                .Include(f => f.Center);
                //.Include(f=>f.Shedules.Where(sch=> sch.Date >= today))
                //.Include(f=> f.Bookings.Where(b=> b.Start >= today));
            if (string.IsNullOrEmpty(keywords)) keywords = "";

            keywords.ToLower().Split(' ')
            .ToList().ForEach(key =>

                query = query.Where(f => f.Name.ToLower().Contains(key)
                                         || f.Center.Location.ToLower().Contains(key)
                                         || f.Center.Neighborhood.ToLower().Contains(key))
            );

            return query;
        }

        /// <summary>
        /// get schedules by location and  filter schedules by provided date
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="date"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public async Task<List<Field>> FullSearchAsync(FilterOptionModel filter)
        {
            IQueryable<Field> fields = Search(filter);


            // crear rangos de  busqueda 4 por defecto
            var ranges = BookingExtensions.BuildTimes(filter).ToList();

            var startTimes = ranges.Select(r => r.Start).ToList();
            var endTimes = ranges.Select(r => r.End).ToList();

            // join por hora exacta de inicio
            // en esta seccion vamos a incluir los estados que consideramos como validos para bloquear el boton reservar
            IQueryable<Booking> _books = Context.Bookings
                .Where(b =>
                 (b.Status == BookingStatus.Reservada || b.Status == BookingStatus.Finalizado)
                && b.Start.HasValue && startTimes.Contains(b.Start.Value)
                //&& b.End.HasValue && endTimes.Contains(b.End.Value) 
                                                );
             

            var data = await (from field in fields.AsNoTracking()
                              join center in Context.Centers.AsNoTracking() on field.CenterId equals center.Id
                              join booking in _books on field.Id equals booking.Idcancha
                              into dfb
                              from defaultBooks in dfb.DefaultIfEmpty()
                              orderby center.Coordinates.Distance(filter.Point)

                              select new
                              {
                                  field = field,
                                  center = center,
                                  Default = defaultBooks,
                              }).ToListAsync();

            var result = data.GroupBy(k => k.field.Id)
                .Select(d => new
                {
                    field = d.FirstOrDefault().field,
                    center = d.FirstOrDefault().center,
                    Books = d.Select(b => b.Default).ToList(),
                }).ToList();


            foreach (var item in result)
            {
                item.field.Distance = item.center.DistanceFromMe(filter.Point);
                var resultItem = result.FirstOrDefault(r => r.field == item.field);
                if (resultItem == null) continue;
                item.field.Center = resultItem.center;
                item.field.Bookings = resultItem.Books.Where(b => b != null).ToList();

                // add available bookings 
                // son 4 botones por defecto si estan disponibles o no
                var books = ranges.Where(t =>
                            !resultItem.Books
                            .Where(b => b != null && b.Start.HasValue).Any(b => b.Start == t.Start && b.End == t.End))
                            .OrderBy(b => b.Start.Value)
                            .Select(t => new Booking()
                            {
                                Start = t.Start,
                                End = t.End,
                                Idcancha = item.field.Id,
                                Field = item.field,

                            }).Take(4).ToList();
                item.field.Bookings.AddRange(books);
                
                // asegurar 4 Botones (por temas de estilo)
                item.field.Bookings = item.field.Bookings.OrderBy(o => o.Start.Value).Take(4).ToList();
              
                //filtrar nuevamente los bookings tomando en cuenta los horarios de inicio del centro.
                var date = item.field.Bookings.Where(b => b.Start.HasValue).Select(b => b.Start.Value.Date).FirstOrDefault();
                //var date = DateTime.Now.Date;

                var centerOpenTime = date.AddHours(item.center.Opentime);

                var centerEndTime = date.AddHours(item.center.Closetime);

                var resultBooks = item.field.Bookings;
                 
                item.field.Bookings = item.field.Bookings
                    .Where(book=> IsOnWorkRange(book,centerOpenTime,centerEndTime)).ToList();

                // TOOD el usuario esta buscando horarios en la madrugada supongo 
                //so , preguntar a Juan Si requiere agregar nuevamente la busqueda o esto sacrificara ligeramente el rendimiento del Search Engine.
                if (!item.field.Bookings.Any())
                {
                    // TODO Agregar nuevamente la busqueda. por el momento mostrarlos como no disponible
                    // El id ni modo , se usa para identificar si esta disponible con la propiedad IsBusy
                    //resultBooks.ForEach(b => b.Id = IsOnCenterWorkRange(b, centerOpenTime, centerEndTime) ? 0: 1);
                    //item.field.Bookings = resultBooks;  
                }
            }

            return result.Select(r => r.field).ToList();

        }


        //private  List<Booking> BuildTimes(List<Field> fields , DateTime? date ,bool forPreview )
        //{
           
        //            var times = new List<Booking>();
        //            // BUILD INTERVALS
        //            foreach (var field in fields)
        //            {
        //                //si es preview solo muestra los 4 mas cercanos
        //                var expectedDate = BookingExtensions.EstimatedTime(date, field.Center.Opentime, forPreview);

        //                var currentTime = forPreview? expectedDate
        //                    :expectedDate.Date.AddHours(field.Center.Opentime);

        //               var finish = expectedDate.AddHours(field.Center.Closetime);

        //                var _times = new List<Booking>();
        //                while (currentTime < finish)
        //                {
        //                    var startAppointment = currentTime;
        //                    var finishApopointment = currentTime.AddMinutes(60);
        //                       _times.Add(new Booking()
        //                    {
        //                        Start = startAppointment,
        //                        End = finishApopointment,
        //                        Idcancha = field.Id,
        //                        Status = BookingStatus.Pendiente,
        //                        Price = field.Cost!=null ? field.Cost.Price:0,
        //                        Field = field,
        //                    });
        //                    currentTime = finishApopointment;
        //                }

        //                times.AddRange(forPreview ? _times.Take(4) : _times);
        //            }
        //            // get unavailables times frpom database (( busy times in field confirmed or not)
                
        //            var unavailableTimes = (from field in fields 
        //                                    join item in times on field.Id equals item.Idcancha
        //                                    join av in Context.Bookings   on new { item.Start, item.End } equals new { av.Start, av.End }
        //                                    into gj from defaultBook in gj.DefaultIfEmpty()
        //                                    select defaultBook
        //                                    ).Where(d=> d!= null).ToList();

        //            unavailableTimes.AddRange(times);

        //            return unavailableTimes.OrderBy(o=>o.Idcancha).GroupBy(g => g.Idcancha)
        //                .SelectMany(g =>
        //                {
        //                    return g.OrderBy(o=>o.Id!=0).GroupBy(gr => new { gr.Start, gr.End })
        //                         .Select(sg => sg.FirstOrDefault()).ToList();
                           
        //                }).ToList();

        //        }

        private bool IsOnWorkRange(Booking book, DateTime open, DateTime close)
        {

            return book.Start >= open && book.End <= close;
        }

        #endregion

        /// <summary>
        /// Take 10 Fields by search
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public Task<List<Field>> SearchAsync(string keywords)
        {
            return Search(keywords??"").ToListAsync();
        }


        public async Task<List<Field>> GetFieldsFromCenterAsync(int? id = null, string keywords = "")
        {
            var centerId = id ?? await Context.GetCenterIdAsync();// ClaimsPrincipal.Current.CenterId();

            var query = centerId.HasValue
                ? Search(keywords).Where(c => c.CenterId == centerId.Value)
                : Search(keywords);

            return await query.OrderBy(f => f.Name).ToListAsync();
        }



        public  async Task AddOrUpdateBooking(Booking booking)
        {
            booking.Userid = UserId;
            booking.OBJECTTYPE = "1";
            // validate locked User Account
            var isLocked = await (from account in Context.AccountAccess.Where(a => a.UserId == UserId.ToString())
                                  join center in Context.Centers.Where(c => c.Fields.Any(f => f.Id == booking.Idcancha))
                                  on account.CenterId equals center.Id
                                  select new { locked = account.Locked }).FirstOrDefaultAsync();


            if (isLocked != null && isLocked.locked) return ;

            if (booking.Id == 0) Context.Bookings.Add(booking);

            else Context.Entry(booking).State = EntityState.Modified;

        }

        public Task<Booking> FindBookAsync(int id)
        {
            return Context.Bookings.FindAsync(id);
        }

        

        
    }
}
