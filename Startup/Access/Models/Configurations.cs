using System;
using System.Collections.Generic;
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
            HasOptional(p => p.Center).WithMany(p => p.Services).HasForeignKey(f => f.IdCenter).WillCascadeOnDelete(true);
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
            
        }
    }

    internal class CostConfig : EntityTypeConfiguration<Cost>
    {
        //public CostConfig()
        //{
        //    HasRequired(p => p.Field)
        //        .WithRequiredPrincipal(p => p.Cost).WillCascadeOnDelete(true);
        //}
    }

    internal class CenterAccountsConfig : EntityTypeConfiguration<CenterAccount>
    {
        public CenterAccountsConfig()
        {
            HasOptional(p=>p.Center).WithMany(c=> c.Employees)
                .HasForeignKey(p=>p.CenterId).WillCascadeOnDelete(false);
        }
    }

    
}
