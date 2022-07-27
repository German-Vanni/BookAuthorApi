using BookAuthor.Api.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookAuthor.Api.Configurations.EF
{
    public class ApiUserEntityConfiguration : IEntityTypeConfiguration<ApiUser>
    {
        string adminId, adminEmail, adminUserName, adminNormUserName, adminNormEmail, adminPassword;
        public ApiUserEntityConfiguration(IConfiguration configuration)
        {
            var adminSec = configuration.GetSection("AdminAccount");
            adminId = adminSec.GetSection("Id").Value;
            adminEmail = adminSec.GetSection("Email").Value;
            adminNormEmail = adminSec.GetSection("NormalizedEmail").Value;
            adminUserName = adminSec.GetSection("UserName").Value;
            adminPassword = adminSec.GetSection("Password").Value;
            adminNormUserName = adminSec.GetSection("NormalizedUserName").Value;

        }

        public void Configure(EntityTypeBuilder<ApiUser> builder)
        {
            var user = new ApiUser
            {
                Id = adminId,
                Email = adminEmail,
                NormalizedEmail = adminNormEmail,
                UserName = adminUserName,
                NormalizedUserName = adminNormUserName,
            };

            PasswordHasher<ApiUser> passwordHasher = new PasswordHasher<ApiUser>();
            user.PasswordHash = passwordHasher.HashPassword(user, adminPassword);

            builder.HasData(user);
        }
    }
}
