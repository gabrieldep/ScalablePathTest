using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ScalablePathTest.Models
{
    public class ScalablePathDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public ScalablePathDbContext()
        {
        }

        public ScalablePathDbContext([NotNull] DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(u =>
            {
                
            });
        }
    }
}
