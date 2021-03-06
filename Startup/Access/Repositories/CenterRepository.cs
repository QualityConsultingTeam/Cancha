﻿using System;
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

                Center = await Context.Centers.Include(p => p.Fields).Include(p => p.Services)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (true)//!Center.ImageField.Any())
            {
                Center.ImageField.Clear();
                Center.ImageField.Add(new ImageField { imgUrl = "/Images/slide0.jpg", header1 = "Las mejores canchas" });
                Center.ImageField.Add(new ImageField { imgUrl = "/Images/slide1.jpg", header1 = "Las mejores canchas" });
                Center.ImageField.Add(new ImageField { imgUrl = "/Images/slide2.jpg", header1 = "Las mejores canchas" });
            }

            if (true) {
                Center.Services.Clear();
                Center.Services.Add(new Service {  Name = "Cafeteria" , Description = "Servico de cafeteria, snack, bebidas", IconName= "fa fa-coffee" });
                Center.Services.Add(new Service { Name = "Gimnasio", Description = "Gimnacio con instructoria profesional", IconName = "fa fa-anchor" });
                Center.Services.Add(new Service { Name = "Clases", Description = "Clases de futbol para diferentes edades", IconName = "fa fa-futbol-o" });
                Center.Services.Add(new Service { Name = "Parqueo", Description = "Amplio parqueo", IconName = "fa fa-car" });
                Center.Services.Add(new Service { Name = "WiFi", Description = "WiFi gratis", IconName = "fa fa-wifi" });
            }

           



            return Center;
        }

        public Task<Center> GetCenter(string userId )
        {                                         
            return (from user in Context.Users.Where(u => u.Id == userId)
                    join center in Context.Centers on user.CenterId equals center.Id
                    select center).FirstOrDefaultAsync();
        }

        public Task<Center> GetCenter()
        {
            var id = UserId.ToString();
            return GetCenter(id);
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

        public async Task<bool> HasLockedUser(string id)
        {
            var centerId = await Context.GetCenterIdAsync();

            if (!centerId.HasValue) return false;

            return await Context.AccountAccess.AnyAsync(u => u.UserId == id && u.CenterId == centerId.Value && u.Locked);  
        }

        public async Task<bool> LockUserForCenter(string userId,bool locked)
        {
            var centerId = await Context.GetCenterIdAsync();

            if (!centerId.HasValue) return false;

            var isLock = await Context.AccountAccess
                .FirstOrDefaultAsync(u => u.UserId == userId&& u.CenterId == centerId.Value);

            if (isLock == null )
            {
                isLock = new AccountAccessLevel()
                {
                    UserId = userId,
                    CenterId = centerId.Value,
                    Locked = locked,
                };

                Context.Entry(isLock).State = EntityState.Added;
            }
            else
            {
                isLock.Locked = locked;
                Context.Entry(isLock).State = EntityState.Modified;
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
