using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using USLabs.TaskManager.Data.Entities;

namespace USLabs.TaskManager.Data.Context
{
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options) { }

        // DbSets for entities
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("YourConnectionStringHere")
                .LogTo(
                    Console.WriteLine,
                    new[] { DbLoggerCategory.Database.Command.Name },
                    LogLevel.Information
                ).EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Category>().ToTable("categories");
            modelBuilder.Entity<TaskItem>().ToTable("task_items");

            // Configurations for properties of User
            modelBuilder.Entity<User>()
                .Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(250);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique()
                .HasDatabaseName("IX_Users_Email");

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Configurations for properties of Category
            modelBuilder.Entity<Category>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Category>()
                .HasIndex(c => new { c.UserId, c.Name })
                .IsUnique()
                .HasDatabaseName("IX_Categories_UserId_Name");

            modelBuilder.Entity<Category>()
                .Property(c => c.Description)
                .HasMaxLength(250);

            modelBuilder.Entity<Category>()
                .Property(c => c.Color)
                .HasMaxLength(7)
                .HasDefaultValue("#007bff");

            modelBuilder.Entity<Category>()
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            // Configurations for properties of TaskItem
            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(500);

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Status)
                .IsRequired()
                .HasConversion<int>();
            modelBuilder.Entity<TaskItem>()
                .Property(t => t.Priority)
                .IsRequired()
                .HasConversion<int>();

            modelBuilder.Entity<TaskItem>()
                .Property(t => t.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<TaskItem>()
                .HasIndex(c => c.UserId)
                .HasDatabaseName("IX_TaskItems_UserId");

            modelBuilder.Entity<TaskItem>()
                .HasIndex(c => c.Status)
                .HasDatabaseName("IX_TaskItems_Status");

            modelBuilder.Entity<TaskItem>()
                .HasIndex(c => c.DueDate)
                .HasDatabaseName("IX_TaskItems_DueDate");

            // Relationships
            modelBuilder.Entity<Category>()
                .HasOne(c => c.User)
                .WithMany(u => u.Categories)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.User)
                .WithMany(u => u.TaskItems)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Category)
                .WithMany(c => c.TaskItems)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}