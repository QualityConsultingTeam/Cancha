using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access.Models;
using System.Data.Entity;

namespace Access.Repositories
{
    public class CenterRepository : BaseRepository<AccessContext, Center>
    {
        public Task<List<Center>> SearchAsync(string query)
        {
            var keys = (query ?? "").ToLower().Split(' ').ToList();

            IQueryable<Center> q = Context.Centers;

            keys.ForEach(k => q = q.Where(c => c.Name.ToLower().Contains(k)));

            return q.ToListAsync();
        }
    }
}
