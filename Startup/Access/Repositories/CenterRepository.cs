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
