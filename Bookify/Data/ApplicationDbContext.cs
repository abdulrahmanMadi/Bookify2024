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
            builder.Entity<BookCategory>().HasKey(x => new { x.CategoryId, x.BookId });
            base.OnModelCreating(builder);
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }






        public DbSet<BookCategory> BooksCategories { get; set; }


    }
}
