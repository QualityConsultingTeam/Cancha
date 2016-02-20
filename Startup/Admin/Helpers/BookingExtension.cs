using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Access.Models;

using Identity;
using Admin.Models;

namespace Admin.Helpers
{
    public static class BookingExtension
    {
        /// <summary>
        /// Update UserInfo model for Scheduler
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<List<BookingViewModel>> UpdateAccountInfoFoScheduler(this List<BookingViewModel> data, Access.AccessContext identityContext)
        {

            var usersIds = data.Select(u => u.UserId).Distinct()
                .Select(u => u.ToString()).ToList();

            // userinfo
            var dat = await (from user in identityContext.Users.Where(u => usersIds.Contains(u.Id))
                             join claim in identityContext.UserClaims on user.Id equals claim.UserId
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
                 Id =  g.FirstOrDefault().user.Id,
                 Name = g.Select(c => c.claim).HasName() ?
                         g.Select(c => c.claim).FinUserName() : g.FirstOrDefault().user.UserName,
                 Phone = g.FirstOrDefault().user.PhoneNumber,
                 Email = g.FirstOrDefault().user.Email,


             }).ToList();


            foreach (var item in data)
            {
                item.UserInfo = users.FirstOrDefault(u => u.Id == item.UserId) ?? new UserInfo();
                item.Start = item.Start;// DateTime.SpecifyKind(item.Start, DateTimeKind.Utc);
                item.End = item.End; //DateTime.SpecifyKind(item.End, DateTimeKind.Utc);
                if (item.UserInfo != null && !string.IsNullOrEmpty(item.UserInfo.Name))
                {
                    item.Title += " " + item.UserInfo.Name + " " + item.UserInfo.Email;
                }
                //if (item.Userid == Guid.Empty) item.Userid = Guid.NewGuid();
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
