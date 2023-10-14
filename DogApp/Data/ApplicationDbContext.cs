using DogApp.ConfigureClasses;
using DogApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DogApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Dog> Dogs => Set<Dog>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new DogConfiguration());
        }
    }
}
