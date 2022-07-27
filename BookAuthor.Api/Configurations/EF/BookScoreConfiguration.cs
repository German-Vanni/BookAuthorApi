using BookAuthor.Api.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookAuthor.Api.Configurations.EF
{
    public class BookScoreConfiguration : IEntityTypeConfiguration<BookScore>
    {
        public void Configure(EntityTypeBuilder<BookScore> builder)
        {
            builder
                .HasKey(bs => new { bs.BookId, bs.UserId });

            builder
                .HasOne(bs => bs.Book)
                .WithMany(b => b.Ratings)
                .HasForeignKey(bs => bs.BookId);

            builder
                .HasOne(bs => bs.User)
                .WithMany(u => u.Ratings)
                .HasForeignKey(bs => bs.UserId);
        }
    }
}
