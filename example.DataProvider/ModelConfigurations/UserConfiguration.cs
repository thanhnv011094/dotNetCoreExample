using example.DataProvider.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace example.DataProvider.ModelConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");
            builder.HasKey(s => s.Id);
            builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            builder.Property(p => p.Name).HasMaxLength(50);
            builder.Property(p => p.Email).HasMaxLength(50);
            builder.HasOne(p => p.Group).WithMany(p => p.Users).HasForeignKey(x => x.GroupId).IsRequired();
        }
    }
}
