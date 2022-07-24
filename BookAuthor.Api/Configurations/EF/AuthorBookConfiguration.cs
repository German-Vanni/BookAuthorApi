using BookAuthor.Api.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookAuthor.Api.Configurations.EF
{
    public class AuthorBookConfiguration : IEntityTypeConfiguration<AuthorBook>
    {
        public void Configure(EntityTypeBuilder<AuthorBook> builder)
        {
            builder
                .HasKey(ab => new { ab.BookId, ab.AuthorId });

            builder
                .HasOne(ab => ab.Book)
                .WithMany(ab => ab.AuthorBooks)
                .HasForeignKey(ab => ab.BookId);

            builder
                .HasOne(ab => ab.Author)
                .WithMany(ab => ab.AuthorBooks)
                .HasForeignKey(ab => ab.AuthorId);
        }
    }
}
