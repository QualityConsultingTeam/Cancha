using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access.Extensions;
using Access.Models;

namespace Access.Repositories
{
    public class FieldRepository: BaseRepository<AccessContext,Field>
    {

        #region Comboboxes
        public async Task<IEnumerable<object>> GetCountriesAsync(string text)
        {
            var countries = !string.IsNullOrEmpty(text)
                ? Context.Countries.Where(c => c.Name.ToLower().Contains(text.ToLower()))
                : Context.Countries;
            return await countries.Select(c => new { id = c.Id, text = c.Name }).ToListAsync();
        }

        public async Task<IEnumerable<object>> GetStatesAsync(int? countryid, string keywords)
        {
            IQueryable<States> states = countryid.HasValue ?
                Context.States.Where(s => s.IdCountry == countryid.Value) :
                Context.States;

            if (!string.IsNullOrEmpty(keywords))
            {
                keywords.ToLower().Split(' ').ToList().ForEach(keyword =>
                {
                    states = states.Where(st => st.State.ToLower().Contains(keyword));
                });
            }

            return await states.Select(s => new { id = s.Id, text = s.State }).ToListAsync();
        }

        public async Task<IEnumerable<object>> GetCitiesAsync(int? stateId, string keywords)
        {
            IQueryable<City> cities = stateId.HasValue ?
               Context.Cities.Where(s => s.StateId == stateId.Value) :
               Context.Cities;

            if (!string.IsNullOrEmpty(keywords))
            {
                keywords.ToLower().Split(' ').ToList()
               .ForEach(keyword => cities = cities.Where(st => st.CityName.ToLower().Contains(keyword)));
            }

            return await cities.Select(s => new { id = s.Id, text = s.CityName }).ToListAsync();
        }

        #endregion

        #region Common Search
        public IQueryable<Field> Search(FilterOptionModel filter)
        {

            IQueryable<Field> query = Search(filter.keywords ?? "");

            query = filter.Point != null
                ? query.OrderBy(f => f.Coordinates.Distance(filter.Point))
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
                                         || f.Location.ToLower().Contains(key)
                                         || f.Neighborhood.ToLower().Contains(key))
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

            var ranges = BookingExtensions.BuildTimes(filter).ToList();

            var startTimes = ranges.Select(r => r.Start).ToList();
            var endTimes = ranges.Select(r => r.End).ToList();

            IQueryable<Booking> _books = Context.Bookings.Where(b => b.Start.HasValue && startTimes.Contains(b.Start.Value) 
                                                //&& b.End.HasValue && endTimes.Contains(b.End.Value) 
                                                );
             

            var data = await (from field in fields.AsNoTracking()
                                join center in Context.Centers.AsNoTracking() on field.CenterId equals center.Id
                                join booking in _books on field.Id equals booking.Idcancha
                                into dfb
                                from defaultBooks in dfb.DefaultIfEmpty()
                                orderby field.Coordinates.Distance(filter.Point)

                                select new
                                {
                                    field = field,
                                    center = center,
                                    Default = defaultBooks,
                                }) .ToListAsync();

            var result = data.GroupBy(k => k.field)
                .Select(d => new
                {
                    field = d.Key,
                    center = d.FirstOrDefault().center,
                    Books = d.Select(b => b.Default).ToList(),
                }).ToList();


            foreach (var item in result)
            {
                item.field.Distance = item.field.DistanceFromMe(filter.Point);
                var resultItem = result.FirstOrDefault(r => r.field == item.field);
                if (resultItem == null) continue;
                item.field.Center = resultItem.center;
                item.field.Bookings = resultItem.Books.Where(b => b != null).ToList();

                // add available bookings
                var books = ranges.Where(t => !resultItem.Books.Where(b=>b!=null). Any(b => b.Start == t.Start && b.End == t.End))
                    .Select(t => new Booking() { Start = t.Start, End = t.End }).ToList();
                item.field.Bookings.AddRange(books);
                item.field.Bookings = item.field.Bookings.OrderBy(o => o.Start).ToList();
            }

            return result.Select(r => r.field).ToList();
            
        }

      

        #endregion


    }
}
