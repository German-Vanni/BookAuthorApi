using BookAuthor.Api.Configurations.EF;
using BookAuthor.Api.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookAuthor.Api.DataAccess
{
    public class AppDbContext : IdentityDbContext<ApiUser>
    {
        public AppDbContext(DbContextOptions opt) : base(opt)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleEntityConfiguration());

            builder.ApplyConfiguration(new AuthorBookConfiguration());

            builder.ApplyConfiguration(new BookScoreConfiguration());

            
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<AuthorBook> AuthorBooks { get; set; }
        public DbSet<BookScore> Ratings { get; set; }
    }
}
