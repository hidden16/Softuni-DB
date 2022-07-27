namespace BookShop.Data
{
    using BookShop.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class BookShopContext : DbContext
    {
        public BookShopContext() { }

        public BookShopContext(DbContextOptions options)
            : base(options) { }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<AuthorBook> AuthorsBooks { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorBook>()
                .HasKey(a => new { a.AuthorId, a.BookId });
            modelBuilder.Entity<AuthorBook>()
                .HasOne(a=>a.Author)
                .WithMany(a=>a.AuthorsBooks);
            modelBuilder.Entity<AuthorBook>()
                .HasOne(a=>a.Book)
                .WithMany(a=>a.AuthorsBooks);
        }
    }
}