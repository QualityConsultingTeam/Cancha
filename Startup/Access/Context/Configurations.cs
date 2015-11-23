using Identity.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models
{
    internal class StatesConfig : EntityTypeConfiguration<States>
    {
        public StatesConfig()
        {
             
        }
    }

    internal class ScheduleConfig : EntityTypeConfiguration<Schedule>
    {
        public ScheduleConfig()
        {
            Property(p => p.RowVersion).IsConcurrencyToken();
            HasRequired(p => p.Field).WithMany(f => f.Shedules)
            .HasForeignKey(p => p.FieldId);
        }
    }


    internal class FieldConfig : EntityTypeConfiguration<Field>
    {
        public FieldConfig()
        {
            
            Property(p => p.RowVersion).IsConcurrencyToken();
            HasOptional(p => p.Center).WithMany(p => p.Fields)
                .HasForeignKey(f => f.CenterId).WillCascadeOnDelete(true);

            
            
        }
    }

    internal class ImageFieldConfig : EntityTypeConfiguration<ImageField> {
        public ImageFieldConfig() {
            Property(p => p.RowVersion).IsConcurrencyToken();
            HasOptional(p => p.Center).WithMany(p => p.ImageField).HasForeignKey(f => f.IdCenter).WillCascadeOnDelete(true);
        }
    }

    internal class ServiceConfig : EntityTypeConfiguration<Service> {
        public ServiceConfig() {
            HasOptional(p => p.Center).WithMany(p => p.Services).HasForeignKey(f => f.IdCenter)
                .WillCascadeOnDelete(false);
        }
    }

    internal class BookingConfig : EntityTypeConfiguration<Booking>
    {
        public BookingConfig()
        {
            Property(p => p.RowVersion).IsConcurrencyToken();
            HasRequired(p => p.Field).WithMany(f => f.Bookings)
                .HasForeignKey(p => p.Idcancha);
        }
    }

    internal class MessagesConfig : EntityTypeConfiguration<Message>
    {
        public MessagesConfig()
        {
            Property(p => p.RowVersion).IsConcurrencyToken();
        }
    }

    internal class PaymentConfig : EntityTypeConfiguration<Payment>
    {

        public PaymentConfig()
        {
            Property(p => p.RowVersion).IsConcurrencyToken();
        }
    }

    internal class CenterConfig : EntityTypeConfiguration<Center>
    {
        public CenterConfig()
        {
            //HasOptional(p => p.AccountAccess).WithMany(a => a.Centers)
            //    .HasForeignKey(p => p.AccountAccessId).WillCascadeOnDelete(false);
        }
    }

    internal class CostConfig : EntityTypeConfiguration<Cost>
    {
        public CostConfig()
        {
            //HasRequired(p => p.Field)
             HasRequired(p => p.Field).WithMany(p => p.Costs).HasForeignKey(c => c.IdCancha).WillCascadeOnDelete(false);
        }
    }

    //internal class CenterAccountsConfig : EntityTypeConfiguration<CenterAccount>
    //{
    //    public CenterAccountsConfig()
    //    {
    //        HasOptional(p=>p.Center).WithMany(c=> c.Employees)
    //            .HasForeignKey(p=>p.CenterId).WillCascadeOnDelete(false);
    //    }
    //}

    internal class AccountAccessLevelConsfig : EntityTypeConfiguration<AccountAccessLevel>
    {
        public AccountAccessLevelConsfig()
        {

            HasRequired(p => p.Center).WithMany(p => p.AccountAccess).HasForeignKey(p => p.CenterId).WillCascadeOnDelete(false);

        }


    }

    public class ApplicationUserConfig : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserConfig()
        {
            HasOptional(p => p.Company).WithMany(c => c.Publishers).HasForeignKey(p => p.CenterId).WillCascadeOnDelete(false);
        }
    }


    public class FeedConfig : EntityTypeConfiguration<Feed>
    {
        public FeedConfig()
        {
            HasRequired(p => p.Category).WithMany(p => p.Feeds).HasForeignKey(p => p.CategoryId).WillCascadeOnDelete(false);
            HasOptional(p => p.User).WithMany(p => p.Feeds).HasForeignKey(p => p.IdPublisher).WillCascadeOnDelete(false);
        }
    }

}
