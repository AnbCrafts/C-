using Microsoft.EntityFrameworkCore;
using WebAPI_PracticeProject_2.Models;

namespace WebAPI_PracticeProject_2.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<QueryUser> QueryUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<QueryUser>(entity =>
            {
                entity.ToTable("QueryUser", "labTest");
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).ValueGeneratedOnAdd();
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FullName).HasMaxLength(150);
                entity.Property(e => e.MobileNo).HasMaxLength(15);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.StateName).HasMaxLength(100);
                entity.Property(e => e.Country).HasMaxLength(100);
                entity.Property(e => e.Gender).HasMaxLength(10);
                entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.Property(e => e.QueryAmount).HasPrecision(18, 2);
            });
        }
    }
}
