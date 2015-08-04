using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access.Models;

namespace Access.Repositories
{
    public class CompanyRepository: BaseRepository<AccessContext,Company>
    {

        public IQueryable<Company> SearchCompanies(string keywords)
        {
            IQueryable<Company> query = Context.Companies;

            if (!string.IsNullOrEmpty(keywords))
            {
                keywords.ToLower().Split(' ').ToList()
                    .ForEach(key=> query= query.Where(c=> c.Name.ToLower().Contains(key) 
                        || c.Address.ToLower().Contains(key)));
            }
            return query;
        }

        public Task<List<Company>> GetCompaniesAsync(string keywords="", int skip = 0, int take = 10)
        {
            IQueryable<Company> query = SearchCompanies(keywords);

            return query.OrderBy(c=> c.Name).Skip(skip).Take(take).ToListAsync();
        }
    }
}
