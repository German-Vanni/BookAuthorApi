using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookAuthor.Api.Configurations.EF
{
    public class UserRoleEntityConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        string adminRoleId, adminId;
        public UserRoleEntityConfiguration(IConfiguration configuration)
        {
            var adminSec = configuration.GetSection("AdminAccount");
            var adminRoleSec = configuration.GetSection("Roles").GetSection("Admin");

            adminRoleId = adminRoleSec.GetSection("Id").Value;
            adminId = adminSec.GetSection("Id").Value;

        }
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {

            builder.HasData(
                new IdentityUserRole<string>
                {
                    UserId = adminId,
                    RoleId = adminRoleId
                });
        }

    }
}

