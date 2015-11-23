using Access.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Repositories
{
    public class CategoriesRepository : BaseRepository<AccessContext, Category>
    {
        public Task<List<Category>> GetAllAsync()
        {
            return Context.Categories.ToListAsync();
        }

        
    }
}
