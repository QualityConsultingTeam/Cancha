﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using Identity.Models;
using Identity.Context;
using Access.Models;
using Access;
using Access.Extensions;
using Identity.Config;

namespace Identity
{
    public class IdentityManagerService
    {
        public IdentityManagerService(ApplicationDbContext context)
        {
            identityContex = context;
        }

        private ApplicationDbContext identityContex;

        public Task<List<IdentityUserViewModel>> GetAllUsersAsync()
        {
            return identityContex.Users.AsNoTracking()
                .Select(u => new IdentityUserViewModel
                {
                    UserName = u.UserName,
                    //ProfilePicture = u.ProfilePicture,
                    //FirstName = u.FirstName,
                    //LastName = u.LastName

                })
                    .ToListAsync();
        }

        private IQueryable<ApplicationUser> CommonSearch(FilterOptionModel filter, AccessContext Context, bool onlyUsers = false)
        {
            IQueryable<ApplicationUser> users = identityContex.Users.Include(i => i.Roles).Include(u => u.Claims);


            identityContex.Users.Add(new ApplicationUser()
            {
                Id=Guid.NewGuid().ToString(),
                UserName="TestUser",
                Email ="TestEmail"
            });

            if (!string.IsNullOrEmpty(filter.role))
            {

                var roleId = identityContex.Roles.FirstOrDefault(r => r.Name == filter.role).Id;
                users = (from user in identityContex.Users
                         join roles in identityContex.UserRoles.Where(r => r.RoleId == roleId) on user.Id equals roles.UserId
                         select user);
            }
            //To Exclude any types of admin
            // los usuarios que no tienen rol asignado es para el cliente final que hace reservas.
            if (string.IsNullOrEmpty(filter.role) && onlyUsers)
            {
                users = (from user in users
                         join userRole in identityContex.UserRoles on user.Id equals userRole.UserId
                         into gu
                         from defaultUserRole in gu.DefaultIfEmpty()
                         where defaultUserRole == null
                         select user);
            }

            if (filter.centerid.HasValue && filter.centerid != 0)
            {
                var idsToFilter =
                  // TODO verificar si el rendimiento es impactado al hacer QUerys separadaas.
                  (Context.AccountAccess.Where(al => al.CenterId == filter.centerid)
                                 .Select(c => c.UserId.ToString()).ToList());
                if (onlyUsers)
                {
                    if (idsToFilter.Any()) users = users.Where(u => idsToFilter.Contains(u.Id));
                }

                else
                {
                    users = users.Where(u =>
                         idsToFilter.Contains(u.Id) ||
                         (u.CenterId.HasValue && u.CenterId == filter.centerid.Value));
                }

            }
            filter.SearchKeys.ForEach(key =>
                         users = users.Where(u => u.UserName.ToLower().Contains(key)
                                             || u.ADDRESS.ToLower().Contains(key)
                                             || u.FirstName.ToLower().Contains(key)
                                             || u.LastName.ToLower().Contains(key)
                                             || u.DUI.ToLower().Contains(key)
                                             || u.Email.ToLower().Contains(key)));

            return users;
        }

        /// <summary>
        ///  Common Filter 
        /// </summary>
        /// <param name="searchKeys"></param>
        /// <returns></returns>
        public async Task<List<Guid>> FilterUsers(FilterOptionModel filter, AccessContext Context)
        {
            var ids = await CommonSearch(filter, Context).Select(u => u.Id).ToListAsync();
            return ids.Select(i => new Guid(i)).ToList();
        }

        public Task<List<ApplicationUser>> GetAllUserNames(AccessContext context)
        {
            return CommonSearch(new FilterOptionModel(), context).Select(u => new ApplicationUser()
            {
                UserName = u.UserName,
                Id = u.Id
            }).ToListAsync();
        }

        public async Task<List<ApplicationUser>> GetUsersAsync(FilterOptionModel filter, AccessContext Context, bool onlyUsers = false)
        {

            IQueryable<ApplicationUser> users = CommonSearch(filter, Context, onlyUsers);


            var accounts = await users.Include(u => u.Claims).OrderBy(u => u.UserName).Skip(filter.Skip).Take(filter.Limit).ToListAsync();

            //refresh profile pictures from extenal login

            foreach (var account in accounts.Where(a => a.Claims.Any()))
            {
                var claim = account.Claims.FirstOrDefault(c => c.ClaimType == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

                if (claim != null)
                {
                    account.ProfilePicture = TokenExtensions.FaceBookProfilePictureFormat(claim.ClaimValue);
                }

                var claimName = account.Claims.FirstOrDefault(c => c.ClaimType == ClaimTypes.Name);
                var givenName = account.Claims.FirstOrDefault(c => c.ClaimType == ClaimTypes.GivenName);
                var phone = account.Claims.FirstOrDefault(c => c.ClaimType == ClaimTypes.MobilePhone);
                if (claimName != null && string.IsNullOrEmpty(account.FirstName)) account.FirstName = claimName.ClaimValue;
                if (givenName != null && string.IsNullOrEmpty(account.LastName)) account.LastName = givenName.ClaimValue;
                if (phone != null && string.IsNullOrEmpty(account.PhoneNumber)) account.PhoneNumber = phone.ClaimValue;

                await identityContex.SaveChangesAsync();
            }

            return accounts;
        }

        public Task<ApplicationUser> GetUserAsync(string userid)
        {
            return identityContex.Users.FirstOrDefaultAsync(u => u.Id == userid);
        }

        public Task<ApplicationUser> UpdateUserAsync(ApplicationUser user)
        {
            return new Task<ApplicationUser>(null);
        }

        public Task<List<string>> GetRolesAsync()
        {
            return identityContex.Roles.Select(r => r.Name).ToListAsync();
        }

        public Task<List<SelectListModel<string>>> GetRolesDataAsync()
        {
            return identityContex.Roles.Select(r =>
                new SelectListModel<string>()
                {
                    Id = r.Id,
                    Text = r.Name
                }).ToListAsync();
        }

        public async Task<bool> InsertOrUpdate(IdentityUserViewModel model, ApplicationUserManager userManager)
        {

            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                user = new ApplicationUser();
                user.Assign(model);


                var result = await userManager.CreateAsync(user, "1234567");
                if (!result.Succeeded) return false;
                model.Id = user.Id;
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.DUI = model.DUI;
                user.PhoneNumber = model.PHONE_2;
                user.ADDRESS = model.ADDRESS;
                user.Category = model.Category;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.DUI = model.DUI;
                user.PHONE_2 = model.PHONE_2;
                user.ProfilePicture = model.ProfilePicture;
                //user.Address = model.Address;
                //user.FirstName = model.FirstName;
                //user.LastName = model.LastName;
                //user.DocumentNum = model.DocumentNum;
                //user.ProfilePicture = model.ProfilePicture;
                await userManager.UpdateAsync(user);
            }

            if (model.ForceChangePassword)
            {
                var tok = await userManager.GeneratePasswordResetTokenAsync(model.Id);

                var result = await userManager.ResetPasswordAsync(model.Id, tok, model.Password);

                if (!result.Succeeded) return false;
            }
            var roles = await userManager.GetRolesAsync(model.Id);

            if (!roles.Any() && !string.IsNullOrEmpty(model.Role))
            {
                var res = await userManager.AddToRoleAsync(model.Id, model.Role);
            }

            if (roles.All(r => r != model.Role) && roles.Any())
            {
                var result = await userManager.AddToRoleAsync(model.Id, model.Role);
            }
            roles.Where(r => r != model.Role).ToList().ForEach(role => userManager.RemoveFromRole(model.Id, role));

            if (model.CenterId != 0)
            {

                var claim = await identityContex.UserClaims.FirstOrDefaultAsync(c => c.UserId == user.Id && c.ClaimType == "CenterId");

                var claims = await userManager.GetClaimsAsync(user.Id);
                if (claim != null)
                {
                    claim.ClaimValue = model.CenterId.Value.ToString();
                    await identityContex.SaveChangesAsync();
                }
                else
                {
                    await userManager.AddClaimAsync(model.Id, new System.Security.Claims.Claim("CenterId", model.CenterId.ToString()));
                }
            }


            return true;
        }


        public Task<List<ApplicationUser>> GetApplicationUsers(string keywords = "")
        {
            IQueryable<ApplicationUser> users = identityContex.Users.Include(i => i.Roles);


            keywords.ToLower().Split(' ').ToList()
                .ForEach(key =>
                    users = users.Where(u => u.UserName.ToLower().Contains(key)
                                        || u.Email.ToLower().Contains(key)
                                        || u.DUI.ToLower().Contains(key)
                                        || u.ADDRESS.ToLower().Contains(key)));

            return users.OrderBy(u => u.UserName)
                .Select(u => new ApplicationUser()
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    DUI = u.DUI,
                    //LastName = u.LastName,
                    //FirstName = u.FirstName,
                })
                .ToListAsync();
        }

        public Task<string> GetMainRoleForUserAsync(string id)
        {
            return (from user in identityContex.Users.Where(u => u.Id == id)
                    join userRoles in identityContex.UserRoles on user.Id equals userRoles.UserId
                    join role in identityContex.Roles on userRoles.RoleId equals role.Id
                    select role.Name).FirstOrDefaultAsync();
        }


        /// <summary>
        /// User info Col. Schedules Admn
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<List<Booking>> UpdateAccountInfo(List<Booking> data)
        {

            var usersIds = data.Select(u => u.Userid).Distinct()
                .Select(u => u.ToString()).ToList();

            // userinfo
            var dat = await (from user in identityContex.Users.Where(u => usersIds.Contains(u.Id))
                             join claim in identityContex.UserClaims on user.Id equals claim.UserId
                                 into df
                             from defaultClaim in df.DefaultIfEmpty()

                             select new
                             {
                                 user = user,
                                 claim = defaultClaim
                             }).ToListAsync();

            // user summary


            var users = dat.GroupBy(u => u.user.Id)
             .Select(g => new UserInfo()
             {
                 Id = new Guid(g.FirstOrDefault().user.Id),
                 Name = g.Select(c => c.claim).HasName() ?
                         g.Select(c => c.claim).FinUserName() : g.FirstOrDefault().user.UserName,
                 Phone = g.FirstOrDefault().user.PhoneNumber,
                 Email = g.FirstOrDefault().user.Email,


             }).ToList();


            foreach (var item in data)
            {
                item.UserInfo = users.FirstOrDefault(u => u.Id == item.Userid) ?? new UserInfo();

            }

            return data;
        }

        /// <summary>
        /// User Schedules Summary
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserInfo> GetUserSummaryAsync(AccessContext context, Guid id)
        {
            var user = await GetUserAsync(id.ToString());

            IQueryable<Booking> query = context.Bookings.Where(b => b.Userid == id);

            var centerid = ClaimsPrincipal.Current.CenterId();

            if (centerid.HasValue) query = query.Where(b => b.Field.CenterId == centerid.Value);

            var data = await query.GroupBy(b => b.Status)
                       .Select(g => new
                       {
                           Label = g.Key.ToString(),
                           Count = g.Count(),
                       }).ToListAsync();


            var inf = new UserInfo()
            {
                Email = user.Email,
                Id = new Guid(user.Id),
                Name = user.Claims.FinUserName(),
                BookingSummary = data.Select(d => new BookingSummary() { Label = d.Label, Count = d.Count }).ToList()
            };

            inf.BookingSummary.Add(new BookingSummary() { Label = "Total", Count = data.Sum(s => s.Count) });

            return inf;

        }




        /// <summary>
        /// Pagination For Grids
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public  async Task<int> GetPageLimit(FilterOptionModel filter, AccessContext context)
        {
            return (await CommonSearch(filter, context).CountAsync()) / filter.Limit + 1;
        }



        /// <summary>
        /// Update User Claims Stored From facebook
        /// </summary>
        /// <param name="user"></param>
        /// <param name="claims"></param>
        /// <returns></returns>
        public async Task InsertOrUpdateUserClaims(string user, List<Claim> claims)
        {
            var userClaims = await identityContex.UserClaims.Where(c => c.UserId == user).ToListAsync();

            // update 
            foreach (var claim in claims)
            {
                var localClaim = userClaims.FirstOrDefault(c => c.ClaimType == claim.Type);
                if (localClaim == null)
                {
                    identityContex.UserClaims.Add(new IdentityUserClaim() { UserId = user, ClaimType = claim.Type, ClaimValue = claim.Value });
                }
                else localClaim.ClaimValue = claim.Value;
            }
            await identityContex.SaveChangesAsync();
        }
    }
}
