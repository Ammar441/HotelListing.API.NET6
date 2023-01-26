using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Data.ConfigDefaultData
{
    public class RoleConfigrationData : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "49c424d4-abf1-4b26-96c0-cd39fa7c7cd5",
                    Name = "Administrator",
                    NormalizedName = "ADMINISTRATOR",
                },
                new IdentityRole
                {
                    Id = "787315f4-34db-408f-981e-12a53f73ea39",
                    Name = "User",
                    NormalizedName = "USER",
                }
                );
        }
    }
}
