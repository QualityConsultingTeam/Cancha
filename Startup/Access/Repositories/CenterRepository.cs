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
        public async Task<Center> GetCenterDetailsAsync(int id) {
            // var Center = FindByIdAsync(id,"Field","ImageField");
            var Center = await Context.Centers.Include(p => p.Fields).Include(p => p.ImageField)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (true)//!Center.ImageField.Any())
            {
                Center.ImageField.Clear();
                Center.ImageField.Add(new ImageField { imgUrl = "/Images/slide0.jpg", header1 = "Las mejores canchas" });
                Center.ImageField.Add(new ImageField { imgUrl = "/Images/slide1.jpg", header1 = "Las mejores canchas" });
                Center.ImageField.Add(new ImageField { imgUrl = "/Images/slide2.jpg", header1 = "Las mejores canchas" });
            }

            return Center;
        }

        

        #region Comoboxes

        public IQueryable<Center> Centers()
        {
            return Context.Centers;
        }

        #endregion 

        public Task<List<Center>> SearchAsync(string query)
        {
            var keys = (query ?? "").ToLower().Split(' ').ToList();

            IQueryable<Center> q = Context.Centers;

            keys.ForEach(k => q = q.Where(c => c.Name.ToLower().Contains(k)));

            return q.ToListAsync();
        }

        private IQueryable<Center> CommonSearch(FilterOptionModel filter)
        {
            IQueryable<Center> q = Context.Centers;

            filter.SearchKeys.ForEach(k => q = q.Where(c => c.Name.ToLower().Contains(k)));

            return q;
        }

        public Task<List<Center>> SearchAsync(FilterOptionModel filter)
        {
            var query = CommonSearch(filter);

            return query.OrderBy(o => o.Name).Skip(filter.Skip).Take(filter.Limit).ToListAsync();

        }
        public async Task UpdateEmployeeCenter(string id, int centerId,Guid? loggedUser)
        {
            var user = await Context.CenterAccounts.FirstOrDefaultAsync(a => a.AccountId == new Guid(id))
                ?? new CenterAccount()
                {
                    AccountId = new Guid(id),
                    CenterId = centerId,
                };
            if (user.Id == 0) Context.CenterAccounts.Add(user);
            else Context.Entry(user).State = EntityState.Modified;

            await SaveAsync(loggedUser);
        }
    }
}
