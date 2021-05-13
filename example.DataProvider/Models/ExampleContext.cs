using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace example.DataProvider.Models
{
    public class ExampleContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("User ID =example;Password=1111;Server=localhost;Port=5433;Database=example_db;Integrated Security=true; Pooling=true;");
        }
    }
}
