namespace Artillery.Data
{
    using Artillery.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class ArtilleryContext : DbContext
    {
        public ArtilleryContext() { }

        public ArtilleryContext(DbContextOptions options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }
        public DbSet<Country> Countries { get; set; }
        public DbSet<CountryGun> CountriesGuns { get; set; }
        public DbSet<Gun> Guns { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Shell> Shells { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Manufacturer>()
                .HasIndex(x => x.ManufacturerName)
                .IsUnique();
            modelBuilder.Entity<Manufacturer>()
                .HasMany(x => x.Guns)
                .WithOne(x => x.Manufacturer);
            modelBuilder.Entity<Shell>()
                .HasMany(x => x.Guns)
                .WithOne(x => x.Shell);
            modelBuilder.Entity<CountryGun>()
                .HasKey(x => new { x.CountryId, x.GunId });
            modelBuilder.Entity<CountryGun>()
                .HasOne(x => x.Gun)
                .WithMany(x => x.CountriesGuns);
            modelBuilder.Entity<CountryGun>()
                .HasOne(x => x.Country)
                .WithMany(x => x.CountriesGuns);
        }
    }
}
