using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Bookify.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasSequence<int>("SerialNumber", schema: "Shared").StartsAt(1000001);
            builder.Entity<BookCopy>().Property(x => x.SerialNumber).HasDefaultValueSql("NEXT VALUE FOR Shared.SerialNumber");

            builder.Entity<BookCategory>().HasKey(x => new { x.CategoryId, x.BookId });
            base.OnModelCreating(builder);
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<BookCopy> Copies { get; set; }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }






        public DbSet<BookCategory> BooksCategories { get; set; }


    }
}
