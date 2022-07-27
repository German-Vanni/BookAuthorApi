using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookAuthor.Api.Configurations.EF
{
    public class RoleEntityConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        string userRoleName, userRoleId, adminRoleName, adminRoleId;
        public RoleEntityConfiguration(IConfiguration configuration)
        {
            var userSec = configuration.GetSection("Roles").GetSection("User");
            var adminSec = configuration.GetSection("Roles").GetSection("Admin");
            userRoleName = userSec.GetSection("Name").Value;
            userRoleId = userSec.GetSection("Id").Value;
            adminRoleName = adminSec.GetSection("Name").Value;
            adminRoleId = adminSec.GetSection("Id").Value;
        }

        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = userRoleId,
                    Name = userRoleName,
                    NormalizedName = userRoleName.ToUpper(),
                },
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = adminRoleName,
                    NormalizedName = adminRoleName.ToUpper(),
                }
            );
        }
    }
}
