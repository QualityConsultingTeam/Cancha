using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access.Models;

namespace Access.Repositories
{
    public class CostRepository : BaseRepository<AccessContext, Cost>
    {
        public Task<List<Cost>> GetByFieldIdAsync(int fieldId)
        {
            return AsQueryable.Where(c => c.IdCancha == fieldId).ToListAsync();
        }
    }
}
