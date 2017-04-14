namespace DataLayer.RTS_CA_Manage
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RTS_CA_Manage : DbContext
    {
        public RTS_CA_Manage()
            : base("name=RTS_CA_Manage")
        {
        }

        public virtual DbSet<alerts> alerts { get; set; }
        public virtual DbSet<cert_centers> cert_centers { get; set; }
        public virtual DbSet<ips> ips { get; set; }
        public virtual DbSet<mails> mails { get; set; }
        public virtual DbSet<root_cert> root_cert { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<user_certs> user_certs { get; set; }
        public virtual DbSet<users> users { get; set; }
        public virtual DbSet<mails_backup> mails_backup { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<alerts>()
                .Property(e => e.server)
                .IsUnicode(false);

            modelBuilder.Entity<alerts>()
                .Property(e => e.error_1)
                .IsUnicode(false);

            modelBuilder.Entity<alerts>()
                .Property(e => e.error_2)
                .IsUnicode(false);

            modelBuilder.Entity<alerts>()
                .Property(e => e.cert_center)
                .IsUnicode(false);

            modelBuilder.Entity<alerts>()
                .Property(e => e.root_serial)
                .IsUnicode(false);

            modelBuilder.Entity<alerts>()
                .Property(e => e.ident)
                .IsUnicode(false);

            modelBuilder.Entity<alerts>()
                .Property(e => e.reason)
                .IsUnicode(false);

            modelBuilder.Entity<alerts>()
                .HasMany(e => e.mails)
                .WithOptional(e => e.alerts)
                .HasForeignKey(e => e.alert_id);

            modelBuilder.Entity<cert_centers>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<cert_centers>()
                .Property(e => e.auth)
                .IsUnicode(false);

            modelBuilder.Entity<cert_centers>()
                .HasMany(e => e.ips)
                .WithRequired(e => e.cert_centers)
                .HasForeignKey(e => e.cert_center)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<cert_centers>()
                .HasMany(e => e.root_cert)
                .WithOptional(e => e.cert_centers)
                .HasForeignKey(e => e.cert_center);

            modelBuilder.Entity<ips>()
                .Property(e => e.addres)
                .IsUnicode(false);

            modelBuilder.Entity<ips>()
                .Property(e => e.phone)
                .IsUnicode(false);

            modelBuilder.Entity<mails>()
                .Property(e => e.text)
                .IsUnicode(false);

            modelBuilder.Entity<root_cert>()
                .Property(e => e.serial)
                .IsUnicode(false);

            modelBuilder.Entity<root_cert>()
                .Property(e => e.hash)
                .IsUnicode(false);

            modelBuilder.Entity<root_cert>()
                .Property(e => e.url)
                .IsUnicode(false);

            modelBuilder.Entity<root_cert>()
                .Property(e => e.url_crl)
                .IsUnicode(false);

            modelBuilder.Entity<root_cert>()
                .Property(e => e.hex)
                .IsUnicode(false);

            modelBuilder.Entity<root_cert>()
                .Property(e => e.ident)
                .IsUnicode(false);

            modelBuilder.Entity<root_cert>()
                .HasMany(e => e.user_certs)
                .WithOptional(e => e.root_cert)
                .HasForeignKey(e => e.root);

            modelBuilder.Entity<user_certs>()
                .Property(e => e.user_serial)
                .IsUnicode(false);

            modelBuilder.Entity<user_certs>()
                .Property(e => e.user_hash)
                .IsUnicode(false);

            modelBuilder.Entity<user_certs>()
                .Property(e => e.root_serial)
                .IsUnicode(false);

            modelBuilder.Entity<user_certs>()
                .Property(e => e.root_hash)
                .IsUnicode(false);

            modelBuilder.Entity<user_certs>()
                .Property(e => e.OID)
                .IsUnicode(false);

            modelBuilder.Entity<user_certs>()
                .HasMany(e => e.users)
                .WithRequired(e => e.user_certs)
                .HasForeignKey(e => e.cert)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<users>()
                .Property(e => e.fio)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.inn)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.org)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .Property(e => e.region_name)
                .IsUnicode(false);

            modelBuilder.Entity<users>()
                .HasMany(e => e.alerts)
                .WithRequired(e => e.users)
                .HasForeignKey(e => e.subject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<mails_backup>()
                .Property(e => e.text)
                .IsUnicode(false);
        }
    }
}
