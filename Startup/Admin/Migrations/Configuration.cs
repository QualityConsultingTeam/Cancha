using Admin.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Admin.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Admin.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Admin.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            if (!context.Roles.Any())
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var role = new IdentityRole
                {
                    Name = "Admin"
                };
                roleManager.Create(role);
            }
             var userStore = new UserStore<ApplicationUser>(context);
                var manager = new ApplicationUserManager(userStore);



                if (!context.Users.Any(u => u.UserName == "Admin" && u.Email == "admin@yopmail.com"))
                {
                    var user = new ApplicationUser
                    {
                        Email = "admin@yopmail.com",
                        UserName = "admin@yopmail.com",

                    };
                    var result =  manager.Create(user, "1234567");

                    if(result.Succeeded) manager.AddToRole(user.Id, "Admin");

                } 


        }
    }
}
