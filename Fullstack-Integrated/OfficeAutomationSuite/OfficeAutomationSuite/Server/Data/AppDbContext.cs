using Microsoft.EntityFrameworkCore;
using OfficeAutomationSuite.Server.Data.Entities;

namespace OfficeAutomationSuite.Server.Data
{
    // Application database context
    // Configures tables and relationships for Entity Framework Core
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // DbSet for Users table
        public DbSet<User> Users { get; set; }

        // DbSet for Cartables table (tasks/assignments)
        public DbSet<Cartable> Cartables { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---------------- User entity configuration ----------------
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(u => u.UserId);
                entity.HasIndex(u => u.Username).IsUnique();

                // Convert bool to int for SQLite
                entity.Property(u => u.IsActive).HasConversion<int>();
                entity.Property(u => u.IsDeleted).HasConversion<int>();
                entity.Property(u => u.IsEmailConfirmed).HasConversion<int>();

                // Default timestamps
                entity.Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(u => u.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // ---------------- Cartable entity configuration ----------------
            modelBuilder.Entity<Cartable>(entity =>
            {
                entity.ToTable("Cartables");
                entity.HasKey(c => c.CartableId);

                // Convert bool to int for SQLite
                entity.Property(c => c.IsCompleted).HasConversion<int>();
                entity.Property(c => c.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Configure relationship to User
                entity.HasOne(c => c.AssignedUser)
                      .WithMany()
                      .HasForeignKey(c => c.AssignedToUserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
