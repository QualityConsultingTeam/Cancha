using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access;
using Access.Models;
using Admin.Helpers;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Access.Extensions;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;

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

        private IQueryable<ApplicationUser> CommonSearch(FilterOptionModel filter,AccessContext Context,bool onlyUsers=false)
        {
            IQueryable<ApplicationUser> users = identityContex.Users.Include(i => i.Roles).Include(u=>u.Claims);

           
            if (!string.IsNullOrEmpty(filter.role))
            {

                var roleId = identityContex.Roles.FirstOrDefault(r => r.Name == filter.role).Id;
                users = (from user in identityContex.Users
                         join roles in identityContex.UserRoles.Where(r => r.RoleId == roleId) on user.Id equals roles.UserId
                         select user);
            }
            //To Exclude any types of admin
            // los usuarios que no tienen rol asignado es para el cliente final que hace reservas.
            if(string.IsNullOrEmpty(filter.role) && onlyUsers)
            {
                users = (from user in users
                         join userRole in identityContex.UserRoles on user.Id equals userRole.UserId
                         into gu
                         from defaultUserRole in gu.DefaultIfEmpty()
                         where defaultUserRole == null
                         select user);
            }

            if (filter.centerid.HasValue && filter.centerid!= 0)
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
                                            || u.DUI.ToLower().Contains(key)
                                            || u.Email.ToLower().Contains(key)));

            return users;
        }

        public Task<List<ApplicationUser>> GetAllUserNames(AccessContext context)
        {
            return CommonSearch(new FilterOptionModel(),context).Select(u=> new ApplicationUser()
            {
                UserName = u.UserName,
                Id = u.Id
            }).ToListAsync();
        }

        public async Task<List<ApplicationUser>> GetUsersAsync(FilterOptionModel filter ,AccessContext Context,bool onlyUsers=false)
        {

            IQueryable<ApplicationUser> users = CommonSearch(filter,Context,onlyUsers);
             

            var accounts = await  users.OrderBy(u => u.UserName).Skip(filter.Skip).Take(filter.Limit).ToListAsync();

            //refresh profile pictures from extenal login

            foreach(var account in  accounts.Where(a=> a.Claims.Any()))
           {
                var claim =  account.Claims.FirstOrDefault(c => c.ClaimType == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");

                if (claim != null)
                {
                    account.ProfilePicture = string.Format("http://graph.facebook.com/{0}/picture?type=large", claim.ClaimValue);
                }
            }

            return   accounts;
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

            if (roles.All(r => r != model.Role) &&roles.Any())
            {
                var result = await userManager.AddToRoleAsync(model.Id, model.Role);
            }
            roles.Where(r => r != model.Role).ToList().ForEach(role => userManager.RemoveFromRole(model.Id, role));

            if(model.CenterId!=0 )
            {

                var claim = await identityContex.UserClaims.FirstOrDefaultAsync(c => c.UserId == user.Id && c.ClaimType == "CenterId");

                var claims = await userManager.GetClaimsAsync(user.Id);
                if (claim!=null)
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
                    Email =  u.Email,
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

        public async Task<List<Booking>> UpdateAccountInfo(List<Booking> data)
        {

            var usersIds = data.Select(u => u.Userid).Distinct()
                .Select(u=> u.ToString()).ToList();

            // userinfo
            var dat = await (from user in identityContex.Users.Where(u => usersIds.Contains(u.Id))
                join claim in identityContex.UserClaims on user.Id equals claim.UserId
                    into df
                from defaultClaim in df.DefaultIfEmpty()
                
                select new
                {
                    user = user,
                    claim = defaultClaim
                } ).ToListAsync();
            
            // user summary
            

               var users = dat.GroupBy(u=> u.user.Id)
                .Select(g=> new UserInfo()
                {
                    Id =new Guid(  g.FirstOrDefault().user.Id),
                    Name =  g.Select(c=> c.claim).HasName()?
                            g.Select(c=> c.claim).FinUserName(): g.FirstOrDefault().user.UserName,
                   Phone = g.FirstOrDefault().user.PhoneNumber,
                   Email = g.FirstOrDefault().user.Email,

                    
                }).ToList();


            foreach (var item in data)
            {
                item.UserInfo = users.FirstOrDefault(u => u.Id == item.Userid) ?? new UserInfo();
               
            }

            return data;
        }


        public async Task<UserInfo> GetUserSummary(AccessContext context, Guid id)
        {
            var user = await GetUserAsync(id.ToString());

            
            var data = await context.Bookings.Where(b => b.Userid == id).GroupBy(b => b.Status)
                       .Select(g => new
                       {
                           Label = g.Key.ToString(),
                           Count =g.Count(),
                       }).ToListAsync();


            var inf= new UserInfo()
            {
                Email = user.Email,
                Id = new Guid(user.Id),
                Name = user.Claims.FinUserName(),
                BookingSummary = data.Select(d => new BookingSummary() { Label = d.Label, Count = d.Count }).ToList()
            };

            inf.BookingSummary.Add(new BookingSummary() { Label = "Total", Count = data.Sum(s=>s.Count) });

            return inf;

        }


        public async Task<List<BookingViewModel>> UpdateAccountInfoFoScheduler(List<BookingViewModel> data)
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
                item.Start = DateTime.SpecifyKind(item.Start, DateTimeKind.Utc);
                item.End= DateTime.SpecifyKind(item.End, DateTimeKind.Utc);
                if(item.UserInfo!=null && !string.IsNullOrEmpty(item.UserInfo.Name)) 
                {
                    item.Title += " "+ item.UserInfo.Name;
                }
                if (item.Userid == Guid.Empty) item.Userid = Guid.NewGuid();
            }
            return data.ToList();
            //return data.Select(d=>
            //{
            //    var book = new BookingViewModel();
            //    book.CopyFrom(d);
            //    book.Title = d.UserInfo.Name +"_ " ;
            //    book.Description = "Reserva de Cancha";

            //    return book;
            //}).ToList();
        }

        internal async Task<int> GetPageLimit(FilterOptionModel filter,AccessContext context)
        {
            return (await CommonSearch(filter,context).CountAsync()) / filter.Limit + 1;
        }
    }
}
