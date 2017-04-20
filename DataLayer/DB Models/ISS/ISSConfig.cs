namespace DataLayer.DB_Models.ISS
{
    using System.Data.Entity;


    public partial class ISSConfig : DbContext
    {
        public ISSConfig(string connectionString)
            : base(connectionString)
        {
            Database.SetInitializer<ISSConfig>(null);
        }

        public virtual DbSet<ISSConfig> Config { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Config>()
                .Property(e => e.ConfigKey)
                .IsUnicode(false);

            modelBuilder.Entity<Config>()
               .Property(e => e.ConfigValue)
               .IsUnicode(false);
        }
    }
}
