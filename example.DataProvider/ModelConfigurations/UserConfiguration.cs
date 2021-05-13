using example.DataProvider.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace example.DataProvider.ModelConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("AppUser");
            //builder.HasKey(s => s.Id);
            //builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            //builder.Property(p => p.FirstName).HasMaxLength(50);
            //builder.Property(p => p.LastName).HasMaxLength(50);
            //builder.Property(p => p.Email).HasMaxLength(50);
        }
    }
}
