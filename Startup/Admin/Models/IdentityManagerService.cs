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

namespace Admin.Models
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

        public Task<List<ApplicationUser>> GetUsersAsync(string role = "", string keywords = "", int skip = 0, int take = 10)
        {

            IQueryable<ApplicationUser> users = identityContex.Users.Include(i => i.Roles);

            if (string.IsNullOrEmpty(keywords)) keywords = "";
            if (!string.IsNullOrEmpty(role))
            {

                var roleId = identityContex.Roles.FirstOrDefault(r => r.Name == role).Id;
                users = (from user in identityContex.Users
                         join roles in identityContex.UserRoles.Where(r => r.RoleId == roleId) on user.Id equals roles.UserId
                         select user);
            }
            keywords.Split(' ').ToList()
                    .ForEach(key =>
                        users = users.Where(u => u.UserName.ToLower().Contains(key)
                                            || u.ADDRESS.ToLower().Contains(key)
                                            || u.DUI. ToLower().Contains(key)
                                            || u.Email.ToLower().Contains(key)));

            return users.OrderBy(u => u.UserName).Skip(skip).ToListAsync();



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

            if (roles.All(r => r != model.Role) &&roles.Any())
            {
                var result = await userManager.AddToRoleAsync(model.Id, model.Role);
            }
            roles.Where(r => r != model.Role).ToList().ForEach(role => userManager.RemoveFromRole(model.Id, role));



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

            IQueryable<Booking> bookings = context.Bookings;

            var data = await (from all in bookings.Where(b => b.Userid == id)
                join pending in bookings.Where(b => b.Status == BookingStatus.Pendiente) on all.Id equals pending.Id
                    into dfp
                from defaultPending in dfp.DefaultIfEmpty()
                join confirmed in bookings.Where(b => b.Status == BookingStatus.Cancelado) on all.Id equals
                    confirmed.Id
                    into dfc
                from defaultConfirmed in dfc.DefaultIfEmpty()
                join canceled in bookings.Where(b => b.Status == BookingStatus.Falta) on all.Id equals canceled.Id
                    into dc
                from defaultCanceled in dc.DefaultIfEmpty()
                select new
                {
                    id = all.Id,

                    pending = defaultPending,
                    confirmed = defaultConfirmed,
                    canceled = defaultCanceled,
                }).OrderBy(i => i.id)
                .GroupBy(i => i.id)
                .Select(g => new
                {
                    all = g.Count(),
                    pending = g.Count(c => c.pending != null),
                    confirmed = g.Count(c => c.confirmed != null),
                    canceled = g.Count(c => c.canceled != null),
                }).FirstOrDefaultAsync();

            return new UserInfo()
            {
                Email = user.Email,
                Id = new Guid(user.Id),
                Name = user.Claims.FinUserName(),
                BookingSummary = new List<BookingSummary>()
                {
                    new BookingSummary() {Label = "Solicitadas", Count = data.all},
                    new BookingSummary() {Label = "Completadas", Count = data.confirmed},
                    new BookingSummary() {Label = "Faltas", Count = data.canceled},
                }
            };


        }


        public async Task<List<BookingViewModel>> UpdateAccountInfoFoScheduler(List<BookingViewModel> data)
        {

            var usersIds = data.Select(u => u.UserId).Distinct()
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
                item.UserInfo = users.FirstOrDefault(u => u.Id == item.UserId) ?? new UserInfo();
                item.Start = DateTime.SpecifyKind(item.Start, DateTimeKind.Utc);
                item.End= DateTime.SpecifyKind(item.End, DateTimeKind.Utc);
                if(item.UserInfo!=null && !string.IsNullOrEmpty(item.UserInfo.Name)) 
                {
                    item.Title = item.UserInfo.Name;
                }
                if (item.UserId == Guid.Empty) item.UserId = Guid.NewGuid();
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

      
    }
}
