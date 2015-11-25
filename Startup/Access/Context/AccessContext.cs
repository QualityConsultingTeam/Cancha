using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Access.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Identity.Models;

namespace Access
{
    public class AccessContext : IdentityDbContext<ApplicationUser>
    {
        public AccessContext()
            : base("DefaultConnection")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public static  AccessContext Create()
        {
            return new AccessContext();
        }

        public DbSet<IdentityUserRole> UserRoles { get; set; }

        public DbSet<IdentityUserClaim> UserClaims { get; set; }

        public DbSet<Agreement> Agreements { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Center> Centers { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<CityIp> CitiesIp { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Cost> Costs { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<Field> Fields { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Schedule> Schedules { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<Setting> Settings { get; set; }

        public DbSet<ImageField> ImageFields { get; set; }

        public DbSet<States> States { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Feed> Feeds { get; set; }

        public DbSet<Category> Categories { get; set; }

       public DbSet<AccountAccessLevel> AccountAccess { get; set; }
        
        //public DbSet<CenterAccount> CenterAccounts { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new ScheduleConfig());
            modelBuilder.Configurations.Add(new FieldConfig());
            modelBuilder.Configurations.Add(new BookingConfig());
            modelBuilder.Configurations.Add(new MessagesConfig());
            modelBuilder.Configurations.Add(new PaymentConfig());
            modelBuilder.Configurations.Add(new ImageFieldConfig());
            modelBuilder.Configurations.Add(new ServiceConfig());
            modelBuilder.Configurations.Add(new CenterConfig());
            modelBuilder.Configurations.Add(new AccountAccessLevelConsfig());
            modelBuilder.Configurations.Add(new CostConfig());
            modelBuilder.Configurations.Add(new FeedConfig());
            modelBuilder.Configurations.Add(new ApplicationUserConfig());
            

        }
    }
}
