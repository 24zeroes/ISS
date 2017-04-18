namespace DataLayer.CubeMonitoring
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CubeMonitoring : DbContext
    {
        

        public CubeMonitoring(string connectionString)
            : base(connectionString)
        {
            Database.SetInitializer<CubeMonitoring>(null);
        }



        public virtual DbSet<ADUsers> ADUsers { get; set; }
        public virtual DbSet<OfficeDCComputers> OfficeDCComputers { get; set; }
        public virtual DbSet<OfficeDCEvents> OfficeDCEvents { get; set; }
        public virtual DbSet<OfficeDCGroups> OfficeDCGroups { get; set; }
        public virtual DbSet<OfficeDCUserGroups> OfficeDCUserGroups { get; set; }
        public virtual DbSet<OfficeDCUsers> OfficeDCUsers { get; set; }
        public virtual DbSet<TelephoneBook> TelephoneBook { get; set; }
        public virtual DbSet<OLD_OfficeDCEvents> OLD_OfficeDCEvents { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ADUsers>()
                .Property(e => e.CN)
                .IsUnicode(false);

            modelBuilder.Entity<ADUsers>()
                .Property(e => e.Sid)
                .IsUnicode(false);

            modelBuilder.Entity<ADUsers>()
                .Property(e => e.GroupName)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCComputers>()
                .Property(e => e.ComputerName)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCComputers>()
                .Property(e => e.ComputerIP)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCEvents>()
                .Property(e => e.EventName)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCEvents>()
                .Property(e => e.LogName)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCEvents>()
                .Property(e => e.Task)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCEvents>()
                .Property(e => e.IpAddress)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCEvents>()
                .Property(e => e.IpPort)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCEvents>()
                .Property(e => e.SubjectUserSid)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCEvents>()
                .Property(e => e.SubjectUserName)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCEvents>()
                .Property(e => e.SubjectDomainName)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCEvents>()
                .Property(e => e.TargetUserSid)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCEvents>()
                .Property(e => e.TargetUserName)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCEvents>()
                .Property(e => e.TargetDomainName)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCEvents>()
                .Property(e => e.PackageName)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCEvents>()
                .Property(e => e.Workstation)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCEvents>()
                .Property(e => e.Computer)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCEvents>()
                .Property(e => e.DnsHostName)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCEvents>()
                .Property(e => e.EventThumb)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCGroups>()
                .Property(e => e.GroupName)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCGroups>()
                .Property(e => e.GroupDescription)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCGroups>()
                .Property(e => e.GroupPath)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCUsers>()
                .Property(e => e.UserFIO)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCUsers>()
                .Property(e => e.UserSID)
                .IsUnicode(false);

            modelBuilder.Entity<OfficeDCUsers>()
                .Property(e => e.UserPath)
                .IsUnicode(false);

            modelBuilder.Entity<TelephoneBook>()
                .Property(e => e.FIO)
                .IsUnicode(false);

            modelBuilder.Entity<TelephoneBook>()
                .Property(e => e.OrgUnit)
                .IsUnicode(false);

            modelBuilder.Entity<TelephoneBook>()
                .Property(e => e.Position)
                .IsUnicode(false);

            modelBuilder.Entity<TelephoneBook>()
                .Property(e => e.Organisation)
                .IsUnicode(false);

            modelBuilder.Entity<TelephoneBook>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<TelephoneBook>()
                .Property(e => e.MobileTelephone)
                .IsUnicode(false);

            modelBuilder.Entity<OLD_OfficeDCEvents>()
                .Property(e => e.EventName)
                .IsUnicode(false);

            modelBuilder.Entity<OLD_OfficeDCEvents>()
                .Property(e => e.LogName)
                .IsUnicode(false);

            modelBuilder.Entity<OLD_OfficeDCEvents>()
                .Property(e => e.Task)
                .IsUnicode(false);

            modelBuilder.Entity<OLD_OfficeDCEvents>()
                .Property(e => e.IpAddress)
                .IsUnicode(false);

            modelBuilder.Entity<OLD_OfficeDCEvents>()
                .Property(e => e.IpPort)
                .IsUnicode(false);

            modelBuilder.Entity<OLD_OfficeDCEvents>()
                .Property(e => e.SubjectUserSid)
                .IsUnicode(false);

            modelBuilder.Entity<OLD_OfficeDCEvents>()
                .Property(e => e.SubjectUserName)
                .IsUnicode(false);

            modelBuilder.Entity<OLD_OfficeDCEvents>()
                .Property(e => e.SubjectDomainName)
                .IsUnicode(false);

            modelBuilder.Entity<OLD_OfficeDCEvents>()
                .Property(e => e.TargetUserSid)
                .IsUnicode(false);

            modelBuilder.Entity<OLD_OfficeDCEvents>()
                .Property(e => e.TargetUserName)
                .IsUnicode(false);

            modelBuilder.Entity<OLD_OfficeDCEvents>()
                .Property(e => e.TargetDomainName)
                .IsUnicode(false);

            modelBuilder.Entity<OLD_OfficeDCEvents>()
                .Property(e => e.PackageName)
                .IsUnicode(false);

            modelBuilder.Entity<OLD_OfficeDCEvents>()
                .Property(e => e.Workstation)
                .IsUnicode(false);

            modelBuilder.Entity<OLD_OfficeDCEvents>()
                .Property(e => e.Computer)
                .IsUnicode(false);

            modelBuilder.Entity<OLD_OfficeDCEvents>()
                .Property(e => e.DnsHostName)
                .IsUnicode(false);

            modelBuilder.Entity<OLD_OfficeDCEvents>()
                .Property(e => e.EventThumb)
                .IsUnicode(false);
        }
    }
}
