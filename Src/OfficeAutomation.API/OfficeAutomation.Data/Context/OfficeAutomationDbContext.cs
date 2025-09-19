using Microsoft.EntityFrameworkCore;
using OfficeAutomation.Core.Entities.Cartables;
using OfficeAutomation.Core.Entities.Security;
using OfficeAutomation.Core.Entities.Tasks;
using OfficeAutomation.Core.Entities.Users;
using OfficeAutomation.Data.Configurations;

namespace OfficeAutomation.Data.Context
{
    // EF Core DbContext for OfficeAutomation database.
    public class OfficeAutomationDbContext : DbContext
    {
        public OfficeAutomationDbContext(DbContextOptions<OfficeAutomationDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<FailedLoginAttempt> FailedLoginAttempts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Cartable> Cartable { get; set; } = default!;
        public DbSet<TaskEntity> Tasks { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Role primary key
            modelBuilder.Entity<Role>()
                .HasKey(r => r.RoleId);

            // Composite key for UserRole
            modelBuilder.Entity<UserRole>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<UserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            // Concurrency token for User
            modelBuilder.Entity<User>()
                .Property(u => u.RowVersion)
                .IsRowVersion();

            // TaskEntity configuration
            modelBuilder.Entity<TaskEntity>(builder =>
            {
                builder.HasKey(t => t.TaskId);

                builder.Property(t => t.Title)
                       .IsRequired()
                       .HasMaxLength(200);

                builder.Property(t => t.Status)
                       .IsRequired()
                       .HasMaxLength(50);

                builder.Property(t => t.RowVersion)
                       .IsRowVersion();

                // Assigned user
                builder.HasOne(t => t.AssignedUser)
                       .WithMany(u => u.Tasks)
                       .HasForeignKey(t => t.AssignedTo)
                       .OnDelete(DeleteBehavior.Restrict);

                // CreatedBy user
                builder.HasOne(t => t.CreatedByUser)
                       .WithMany()
                       .HasForeignKey(t => t.CreatedBy)
                       .OnDelete(DeleteBehavior.Restrict);

                // CompletedBy user
                builder.HasOne(t => t.CompletedByUser)
                       .WithMany()
                       .HasForeignKey(t => t.CompletedBy)
                       .OnDelete(DeleteBehavior.Restrict);
            });

            // Cartable configuration
            modelBuilder.Entity<Cartable>(builder =>
            {
                builder.HasKey(c => c.CartableId);

                builder.HasOne(c => c.User)
                       .WithMany()
                       .HasForeignKey(c => c.UserId)
                       .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(c => c.Task)
                       .WithMany()
                       .HasForeignKey(c => c.TaskId)
                       .OnDelete(DeleteBehavior.Restrict);

                builder.Property(c => c.RowVersion)
                       .IsRowVersion();
            });

            // FailedLoginAttempt configuration
            modelBuilder.Entity<FailedLoginAttempt>(builder =>
            {
                builder.HasKey(f => f.AttemptId);

                builder.HasOne(f => f.User)
                       .WithMany(u => u.FailedLoginAttempts)
                       .HasForeignKey(f => f.UserId)
                       .OnDelete(DeleteBehavior.Cascade);
            });

            // AuditLog configuration
            modelBuilder.Entity<AuditLog>(builder =>
            {
                builder.HasKey(a => a.LogId);
                builder.Property(a => a.Action).IsRequired().HasMaxLength(200);
                builder.Property(a => a.Details).HasColumnType("nvarchar(max)");
                builder.Property(a => a.IPAddress).HasMaxLength(45);
                builder.Property(a => a.Timestamp)
                       .HasDefaultValueSql("sysutcdatetime()")
                       .IsRequired();

                builder.HasOne(a => a.User)
                       .WithMany()
                       .HasForeignKey(a => a.UserId)
                       .OnDelete(DeleteBehavior.SetNull);
            });

            // UserSession configuration
            modelBuilder.Entity<UserSession>()
                .HasKey(s => s.SessionId);

            modelBuilder.Entity<UserSession>()
                .HasOne(s => s.User)
                .WithMany(u => u.UserSessions)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Apply RefreshToken configuration
            modelBuilder.ApplyConfiguration(new RefreshTokenConfiguration());
        }
    }
}
