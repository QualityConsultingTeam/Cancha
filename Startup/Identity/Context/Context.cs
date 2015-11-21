using Identity.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Context
{
    //public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    //{
    //    public ApplicationDbContext()
    //        : base("IdentityData", throwIfV1Schema: false)
    //    {
    //        this.Configuration.LazyLoadingEnabled = false;
    //    }

    //    public DbSet<IdentityUserRole> UserRoles { get; set; }

    //    public DbSet<IdentityUserClaim> UserClaims { get; set; }

    //    public static ApplicationDbContext Create()
    //    {
    //        return new ApplicationDbContext();
    //    }
    //    protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
    //    {
    //        base.OnModelCreating(modelBuilder);
    //        //// to avoid the "has no keys" errors when running Update-Database on PM
    //        //modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id).ToTable("AspNetRoles");

    //        //EntityTypeConfiguration<IdentityUser> table =  modelBuilder.Entity<IdentityUser>().ToTable("AspNetUsers");

    //        //table.HasMany(c => c.Claims).WithOptional().HasForeignKey(c => c.UserId);
    //        //table.HasMany(c => c.Logins).WithOptional().HasForeignKey(c => c.UserId);
    //        //table.HasMany(c => c.Roles).WithOptional().HasForeignKey(c => c.UserId);

    //        //modelBuilder.Entity<IdentityUserLogin>().HasKey(l => new { l.UserId, l.LoginProvider, l.ProviderKey }).ToTable("AspNetUserLogins");
    //        //modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId }).ToTable("AspNetUserRoles");
    //        //modelBuilder.Entity<IdentityUserClaim>().HasKey(k=>k.Id).ToTable("AspNetUserClaims");



    //        //    modelBuilder.Entity<IdentityUserClaim>().ToTable("AspNetUserClaims");

    //    }
    //}
}
