using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HR.LeaveManagement.Identity.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "66A2C61F-7B5A-4448-8FB8-BF621B05FDA1",
                    UserId = "9AFD2EDB-BCFA-4B72-BCD2-5F3EDC378310"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "7696A960-C764-4D54-B59C-1CB275AB8579",
                    UserId = "A4135D9E-14BB-48B2-A14E-E5F463986A58"
                }
            );
        }
    }
}
