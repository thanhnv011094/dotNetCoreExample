using example.DataProvider.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace example.DataProvider.ModelConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("AppRole");
            //builder.HasKey(s => s.Id);
            //builder.Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
            //builder.Property(p => p.Name).HasMaxLength(50);
            //builder.Property(p => p.Description).HasMaxLength(200);
        }
    }
}
