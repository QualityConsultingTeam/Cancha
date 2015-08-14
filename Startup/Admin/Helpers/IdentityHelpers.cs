using Access.Extensions;
using Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin
{
    public static class IdentityHelpers
    {

        public static IdentityUserViewModel ToIdentityUserViewModel(this ApplicationUser model)
        {
            var i = new IdentityUserViewModel();
            i.Assign(model);

            return i;
        }
        public static async Task<IdentityUserViewModel> ToIdentityUserViewModelAsync(this ApplicationUser model, IdentityManagerService service)
        {
            var i = new IdentityUserViewModel();
            i.Assign(model);

            if (service != null) i.Role = await service.GetMainRoleForUserAsync(model.Id) ?? "";

            return i;
        }


        public static List<IdentityUserViewModel> ToIdentityUserViewModel(this List<ApplicationUser> model)
        {
            return model.Select(m => m.ToIdentityUserViewModel()).ToList();
        }
 

     

        //public static async Task<List<CheckIn>> UpdateUserPictureAsync(this List<CheckIn> model,
        //    Func<List<Guid>, Task<List<IdentityUserViewModel>>> updateFunc)
        //{
        //    if (updateFunc == null) return model;

        //    var mechanics = (await updateFunc(model.Select(m => m.AssignedTo).ToList()));

        //    foreach (var checkIn in model.Where(checkIn => mechanics.Any(m => m.Id == checkIn.AssignedTo.ToString())))
        //    {
        //        checkIn.MechanicPicture = mechanics.FirstOrDefault(m => m.Id == checkIn.AssignedTo.ToString()).ProfilePicture;
        //    }

        //    return model;
        //}
    }
}
