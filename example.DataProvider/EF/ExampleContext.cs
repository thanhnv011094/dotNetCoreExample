using System.Reflection;
using example.DataProvider.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace example.DataProvider.EF
{
    public class ExampleContext : IdentityDbContext<User, Role, int>
    {
        //public DbSet<User> Users { get; set; }
        //public DbSet<Group> Groups { get; set; }

        public ExampleContext(DbContextOptions<ExampleContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            builder.Entity<IdentityUserClaim<int>>().ToTable("AppUserClaim")/*.HasKey(x => x.Id)*/;
            builder.Entity<IdentityUserRole<int>>().ToTable("AppUserRole")/*.HasKey(x => new { x.UserId, x.RoleId })*/;
            builder.Entity<IdentityUserLogin<int>>().ToTable("AppUserLogin")/*.HasKey(x => x.UserId)*/;
            builder.Entity<IdentityRoleClaim<int>>().ToTable("AppRoleClaim")/*.HasKey(x => x.Id)*/;
            builder.Entity<IdentityUserToken<int>>().ToTable("AppUserToken")/*.HasKey(x => x.UserId)*/;
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseNpgsql("User ID =example;Password=1111;Server=localhost;Port=5432;Database=example_db;Integrated Security=true; Pooling=true;");
        //}
    }
}
