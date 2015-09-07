using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access.Models;
using System.Data.Entity;
using System.Security.Claims;
using Access.Extensions;

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

        public Task<bool> HasLockedUser(string id)
        {
            var centerId = ClaimsPrincipal.Current.CenterId();

            if (!centerId.HasValue) return Task.FromResult(false);

            return Context.UsersCenter.AnyAsync(u => u.UserId == new Guid(id) && u.CenterId == centerId.Value);
             

        }

        public async Task<bool> LockUserForCenter(string userId,bool locked)
        {
            var centerId = ClaimsPrincipal.Current.CenterId();

            if (!centerId.HasValue) return false;

            var isLock = await Context.UsersCenter
                .FirstOrDefaultAsync(u => u.UserId == new Guid(userId)&& u.CenterId == centerId.Value);

            if (isLock == null && locked)
            {
                isLock = new UserCenter()
                {
                    UserId = new Guid(userId),
                    CenterId = centerId.Value,
                    Locked = locked,
                };
            }
            else
            {
                if (locked)
                {
                    isLock.Locked = locked;
                    Context.Entry(isLock).State = EntityState.Modified;
                }
                else Context.Entry(isLock).State = EntityState.Deleted;
                
            }
             
            await SaveAsync();
            return true;
        }

        //public async Task UpdateEmployeeCenter(string id, int centerId,Guid? loggedUser)
        //{
        //    var user = await Context.CenterAccounts.FirstOrDefaultAsync(a => a.AccountId == new Guid(id))
        //        ?? new CenterAccount()
        //        {
        //            AccountId = new Guid(id),
        //            CenterId = centerId,
        //        };
        //    if (user.Id == 0) Context.CenterAccounts.Add(user);
        //    else Context.Entry(user).State = EntityState.Modified;

        //    await SaveAsync(loggedUser);
        //}
    }
}
