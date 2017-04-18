namespace DataLayer.DB_Models.Logging
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Logger : DbContext
    {
        public Logger(string connectionString)
            : base(connectionString)
        {
            Database.SetInitializer<Logger>(null);
        }

        public virtual DbSet<CommonLogs> CommonLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommonLogs>()
                .Property(e => e.message)
                .IsUnicode(false);

            modelBuilder.Entity<CommonLogs>()
                .Property(e => e.application)
                .IsUnicode(false);

            modelBuilder.Entity<CommonLogs>()
                .Property(e => e.context)
                .IsUnicode(false);

            modelBuilder.Entity<CommonLogs>()
                .Property(e => e.category)
                .IsUnicode(false);
        }
    }
}
