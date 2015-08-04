namespace EnLaCanchaAccess.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class AccessContext : DbContext
    {
        public AccessContext()
            : base("name=AccessContext")
        {
        }

        public virtual DbSet<ADMINUSER> ADMINUSER { get; set; }
        public virtual DbSet<AGREEMENT> AGREEMENT { get; set; }
        public virtual DbSet<BOOKING> BOOKING { get; set; }
        public virtual DbSet<CANCHA> CANCHA { get; set; }
        public virtual DbSet<CENTER> CENTER { get; set; }
        public virtual DbSet<COMPANY> COMPANY { get; set; }
        public virtual DbSet<COST> COST { get; set; }
        public virtual DbSet<MESSAGES> MESSAGES { get; set; }
        public virtual DbSet<OBJECTTYPES> OBJECTTYPES { get; set; }
        public virtual DbSet<PAYMENT> PAYMENT { get; set; }
        public virtual DbSet<SCHEDULES> SCHEDULES { get; set; }
        public virtual DbSet<SERVICES> SERVICES { get; set; }
        public virtual DbSet<SETTING> SETTING { get; set; }
        public virtual DbSet<USER> USER { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ADMINUSER>()
                .Property(e => e.IDENTRY)
                .HasPrecision(18, 0);

            modelBuilder.Entity<ADMINUSER>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<ADMINUSER>()
                .Property(e => e.NICKNAME)
                .IsUnicode(false);

            modelBuilder.Entity<ADMINUSER>()
                .Property(e => e.PASSWORD)
                .IsUnicode(false);

            modelBuilder.Entity<ADMINUSER>()
                .Property(e => e.DUI)
                .IsUnicode(false);

            modelBuilder.Entity<ADMINUSER>()
                .Property(e => e.PHONE_1)
                .IsUnicode(false);

            modelBuilder.Entity<ADMINUSER>()
                .Property(e => e.PHONE_2)
                .IsUnicode(false);

            modelBuilder.Entity<ADMINUSER>()
                .Property(e => e.EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<ADMINUSER>()
                .Property(e => e.PROFILE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<ADMINUSER>()
                .Property(e => e.ADDRESS)
                .IsUnicode(false);

            modelBuilder.Entity<ADMINUSER>()
                .Property(e => e.CATEGORY)
                .HasPrecision(18, 6);

            modelBuilder.Entity<ADMINUSER>()
                .Property(e => e.STATUS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<AGREEMENT>()
                .Property(e => e.IDENTRY)
                .HasPrecision(18, 0);

            modelBuilder.Entity<AGREEMENT>()
                .Property(e => e.OBJECTTYPE)
                .IsUnicode(false);

            modelBuilder.Entity<AGREEMENT>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<AGREEMENT>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<AGREEMENT>()
                .Property(e => e.COMMENTS)
                .IsUnicode(false);

            modelBuilder.Entity<AGREEMENT>()
                .Property(e => e.STATUS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BOOKING>()
                .Property(e => e.IDENTRY)
                .HasPrecision(18, 0);

            modelBuilder.Entity<BOOKING>()
                .Property(e => e.OBJECTTYPE)
                .IsUnicode(false);

            modelBuilder.Entity<BOOKING>()
                .Property(e => e.INVOICE)
                .IsUnicode(false);

            modelBuilder.Entity<BOOKING>()
                .Property(e => e.STATUS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<BOOKING>()
                .Property(e => e.PRICE)
                .HasPrecision(18, 6);

            modelBuilder.Entity<BOOKING>()
                .Property(e => e.DOWNPAYMENT)
                .HasPrecision(18, 6);

            modelBuilder.Entity<BOOKING>()
                .Property(e => e.PENDING)
                .HasPrecision(18, 6);

            modelBuilder.Entity<BOOKING>()
                .Property(e => e.TEAM_1)
                .IsUnicode(false);

            modelBuilder.Entity<BOOKING>()
                .Property(e => e.TEAM_2)
                .IsUnicode(false);

            modelBuilder.Entity<CANCHA>()
                .Property(e => e.IDENTRY)
                .HasPrecision(18, 0);

            modelBuilder.Entity<CANCHA>()
                .Property(e => e.OBJECTTYPE)
                .IsUnicode(false);

            modelBuilder.Entity<CANCHA>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<CANCHA>()
                .Property(e => e.LOCATION)
                .IsUnicode(false);

            modelBuilder.Entity<CANCHA>()
                .Property(e => e.COORDINATES)
                .IsUnicode(false);

            modelBuilder.Entity<CANCHA>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<CANCHA>()
                .Property(e => e.COMMENTS)
                .IsUnicode(false);

            modelBuilder.Entity<CANCHA>()
                .Property(e => e.NEIGHBORHOOD)
                .IsUnicode(false);

            modelBuilder.Entity<CANCHA>()
                .Property(e => e.LENGTH)
                .HasPrecision(18, 6);

            modelBuilder.Entity<CANCHA>()
                .Property(e => e.WIDTH)
                .HasPrecision(18, 6);

            modelBuilder.Entity<CANCHA>()
                .Property(e => e.GRADE)
                .HasPrecision(18, 6);

            modelBuilder.Entity<CANCHA>()
                .Property(e => e.STATUS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<CENTER>()
                .Property(e => e.IDENTRY)
                .HasPrecision(18, 0);

            modelBuilder.Entity<CENTER>()
                .Property(e => e.OBJECTTYPE)
                .IsUnicode(false);

            modelBuilder.Entity<CENTER>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<CENTER>()
                .Property(e => e.LOCATION)
                .IsUnicode(false);

            modelBuilder.Entity<CENTER>()
                .Property(e => e.COORDINATES)
                .IsUnicode(false);

            modelBuilder.Entity<CENTER>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<CENTER>()
                .Property(e => e.NEIGHBORHOOD)
                .IsUnicode(false);

            modelBuilder.Entity<CENTER>()
                .Property(e => e.ADMINISTRATOR)
                .IsUnicode(false);

            modelBuilder.Entity<CENTER>()
                .Property(e => e.PERSONCONTACT)
                .IsUnicode(false);

            modelBuilder.Entity<CENTER>()
                .Property(e => e.PHONE_1)
                .IsUnicode(false);

            modelBuilder.Entity<CENTER>()
                .Property(e => e.PHONE_2)
                .IsUnicode(false);

            modelBuilder.Entity<CENTER>()
                .Property(e => e.EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<CENTER>()
                .Property(e => e.STATUS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<COMPANY>()
                .Property(e => e.OBJECTTYPE)
                .IsUnicode(false);

            modelBuilder.Entity<COMPANY>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<COMPANY>()
                .Property(e => e.NIT)
                .IsUnicode(false);

            modelBuilder.Entity<COMPANY>()
                .Property(e => e.PHONE1)
                .IsUnicode(false);

            modelBuilder.Entity<COMPANY>()
                .Property(e => e.PHONE2)
                .IsUnicode(false);

            modelBuilder.Entity<COMPANY>()
                .Property(e => e.ADDRESS)
                .IsUnicode(false);

            modelBuilder.Entity<COMPANY>()
                .Property(e => e.PERSONCONTACT)
                .IsUnicode(false);

            modelBuilder.Entity<COMPANY>()
                .Property(e => e.STATUS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<COST>()
                .Property(e => e.IDENTRY)
                .HasPrecision(18, 0);

            modelBuilder.Entity<COST>()
                .Property(e => e.OBJECTTYPE)
                .IsUnicode(false);

            modelBuilder.Entity<COST>()
                .Property(e => e.PRICE)
                .HasPrecision(18, 6);

            modelBuilder.Entity<COST>()
                .Property(e => e.DOWNPAYMENT)
                .HasPrecision(18, 6);

            modelBuilder.Entity<MESSAGES>()
                .Property(e => e.IDENTRY)
                .HasPrecision(18, 0);

            modelBuilder.Entity<MESSAGES>()
                .Property(e => e.OBJECTTYPE)
                .IsUnicode(false);

            modelBuilder.Entity<MESSAGES>()
                .Property(e => e.STATUS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<MESSAGES>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<MESSAGES>()
                .Property(e => e.COMMENTS)
                .IsUnicode(false);

            modelBuilder.Entity<OBJECTTYPES>()
                .Property(e => e.OBJECTTYPE)
                .IsUnicode(false);

            modelBuilder.Entity<OBJECTTYPES>()
                .Property(e => e.CODE)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<OBJECTTYPES>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<OBJECTTYPES>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<OBJECTTYPES>()
                .Property(e => e.STATUS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<PAYMENT>()
                .Property(e => e.IDENTRY)
                .HasPrecision(18, 0);

            modelBuilder.Entity<PAYMENT>()
                .Property(e => e.OBJECTTYPE)
                .IsUnicode(false);

            modelBuilder.Entity<PAYMENT>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<PAYMENT>()
                .Property(e => e.COMMENTS)
                .IsUnicode(false);

            modelBuilder.Entity<PAYMENT>()
                .Property(e => e.AMOUNT)
                .HasPrecision(18, 6);

            modelBuilder.Entity<SCHEDULES>()
                .Property(e => e.IDENTRY)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SCHEDULES>()
                .Property(e => e.OBJECTTYPE)
                .IsUnicode(false);

            modelBuilder.Entity<SERVICES>()
                .Property(e => e.IDENTRY)
                .HasPrecision(18, 0);

            modelBuilder.Entity<SERVICES>()
                .Property(e => e.OBJECTTYPE)
                .IsUnicode(false);

            modelBuilder.Entity<SERVICES>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<SERVICES>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<SERVICES>()
                .Property(e => e.STATUS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SETTING>()
                .Property(e => e.OBJECTTYPE)
                .IsUnicode(false);

            modelBuilder.Entity<SETTING>()
                .Property(e => e.CODE)
                .IsUnicode(false);

            modelBuilder.Entity<SETTING>()
                .Property(e => e.DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<SETTING>()
                .Property(e => e.STATUS)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.IDENTRY)
                .HasPrecision(18, 0);

            modelBuilder.Entity<USER>()
                .Property(e => e.OBJECTTYPE)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.NICKNAME)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.PASSWORD)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.DUI)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.PHONE_1)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.PHONE_2)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.EMAIL)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.ADDRESS)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.COORDINATES)
                .IsUnicode(false);

            modelBuilder.Entity<USER>()
                .Property(e => e.CATEGORY)
                .HasPrecision(18, 6);

            modelBuilder.Entity<USER>()
                .Property(e => e.STATUS)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
