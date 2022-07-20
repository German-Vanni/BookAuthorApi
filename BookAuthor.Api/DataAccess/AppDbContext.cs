using BookAuthor.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace BookAuthor.Api.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions opt) : base(opt)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Book>().HasData(
                new Book
                {
                    Id = 1,
                    Title = "Title 1",
                    ISBN10 = "4242421421",
                    ISBN13 = "4242421421123",
                    AuthorId = 1
                },
                new Book
                {
                    Id = 2,
                    Title = "Title 2",
                    ISBN10 = "4242421421",
                    ISBN13 = "4242421421123",
                    AuthorId = 1
                },
                new Book
                {
                    Id = 3,
                    Title = "Title 3",
                    ISBN10 = "4242421421",
                    ISBN13 = "4242421421123",
                    AuthorId = 2
                },
                new Book
                {
                    Id = 4,
                    Title = "Title 4",
                    ISBN10 = "4242421421",
                    ISBN13 = "4242421421123",
                    AuthorId = 3
                });
            builder.Entity<Author>().HasData(
                new Author
                {
                    Id = 1,
                    Name = "Author One",
                    BirthDay = DateTime.Now.AddDays(-365 * 50)
                },
                new Author
                {
                    Id = 2,
                    Name = "Author Two",
                    BirthDay = DateTime.Now.AddDays(-365 * 35)
                },
                new Author
                {
                    Id = 3,
                    Name = "Author Three",
                    BirthDay = DateTime.Now.AddDays(- 365 *  30)
                }
                );
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}
