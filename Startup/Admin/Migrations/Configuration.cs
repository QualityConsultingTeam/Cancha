using Admin.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Admin.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Security.Claims;

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

            if(!context.Roles.Any(a=> a.Name == "Manager"))
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var role = new IdentityRole
                {
                    Name = "Manager"
                };
                roleManager.Create(role);
            }
           

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
            else
            {
                var user = userStore.FindByEmailAsync("admin@yopmail.com").Result;
                
                  userStore.AddClaimAsync(user, claim: new Claim(ClaimTypes.Role.ToString(), "Admin")).Start();
            }

            if (context.Users.Count() < 10)
            {
                foreach (var user in TestUsers())
                {
                    var result = manager.Create(user, "1234567");
                }
            }

            var toUpdateProfiles = context.Users.Where(u => string.IsNullOrEmpty(u.ProfilePicture)).ToList();



            

        }

        private List<ApplicationUser> TestUsers()
        {
            return new List<ApplicationUser>()
            {
                new ApplicationUser("Luke@yopmail.com","Luke","Doe"),
                new ApplicationUser("Diego@yopmail.com","Adolfo","Calles"),
                new ApplicationUser("John@yopmail.com","John","Doe"),
                new ApplicationUser("Bruce.wayne@yopmail.com","Bruce","Wayne"),
                new ApplicationUser("steven.strange@yopmail.com","Steven","Strange"),
                new ApplicationUser("peter.parker@yopmail.com","Peter","Parker"),
                new ApplicationUser("james.buck@yopmail.com","James","Buck"),
                new ApplicationUser("Steve.Rogers@yopmail.com","Steve","Rogers"),
                new ApplicationUser("Luke00@yopmail.com","Mario","Doe"),
                new ApplicationUser("Diego00@yopmail.com","Ramon","Calles"),
                new ApplicationUser("John00@yopmail.com","Erick","Doe"),
                new ApplicationUser("Bruce.wayne00@yopmail.com","Carlos","Wayne"),
                new ApplicationUser("steven.strange00@yopmail.com","Kinchiro","Strange"),
                new ApplicationUser("peter.parker00@yopmail.com","Alexr","Parker"),
                new ApplicationUser("james.buck00@yopmail.com","Julian","Buck"),
                new ApplicationUser("r.parras@yopmail.com","Roberto","Parras"),
                new ApplicationUser("veronica.garcia@yopmail.com","veronica","garcia"),
                new ApplicationUser("paola.martinez@yopmail.com","paola","martinez"),
                new ApplicationUser("jeanet.molina@yopmail.com","jeanet","molina"),
                new ApplicationUser("sonia.gonzalez@yopmail.com","sonia","gonzales"),

                // Test Yakiris Makiris
            };
        }
    }
}
