using System.Net;
using Azure.Core;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using USLabs.TaskManager.Data.Entities;
using USLabs.TaskManager.Shared.Enums;

namespace USLabs.TaskManager.Data.Context
{
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options) { }

        // DbSets for entities
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Category>().ToTable("categories");
            modelBuilder.Entity<TaskItem>().ToTable("task_items");

            // Configurations for properties of User
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedNever();

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
                .Property(c => c.Id)
                .ValueGeneratedNever();

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
                .Property(t => t.Id)
                .ValueGeneratedNever();

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
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.Category)
                .WithMany(c => c.TaskItems)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed data
            var seedData = SeedDataMaster();
            modelBuilder.Entity<User>().HasData(seedData.Item1);
            modelBuilder.Entity<Category>().HasData(seedData.Item2);
            modelBuilder.Entity<TaskItem>().HasData(seedData.Item3);
        }

        private Tuple<User[], Category[], TaskItem[]> SeedDataMaster()
        {
            var users = new Faker<User>()
                .RuleFor(u => u.Id, f => Guid.NewGuid())
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.PasswordHash, f => f.Internet.Password())
                .RuleFor(u => u.CreatedAt, f => DateTime.UtcNow)
                .Generate(4);

            var categories = new List<Category>();
            var categoryFaker = new Faker<Category>()
                .RuleFor(c => c.Id, f => Guid.NewGuid())
                .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0])
                .RuleFor(c => c.Description, f => f.Lorem.Sentence())
                .RuleFor(c => c.Color, f => f.Internet.Color())
                .RuleFor(c => c.CreatedAt, f => DateTime.UtcNow);

            foreach (var user in users)
            {
                // 1 categoría por usuario (puedes ajustar este número)
                var userCategories = categoryFaker.Clone()
                    .RuleFor(c => c.UserId, _ => user.Id)
                    .Generate(1);

                categories.AddRange(userCategories);
            }

            var taskItems = new List<TaskItem>();
            var taskFaker = new Faker<TaskItem>()
                .RuleFor(t => t.Id, f => Guid.NewGuid())
                .RuleFor(t => t.Title, f => f.Lorem.Sentence(3))
                .RuleFor(t => t.Description, f => f.Lorem.Paragraph())
                .RuleFor(t => t.Status, f => f.PickRandom<TaskStatusU>())
                .RuleFor(t => t.Priority, f => f.PickRandom<Priority>())
                .RuleFor(t => t.DueDate, f => f.Date.Future())
                .RuleFor(t => t.CreatedAt, f => DateTime.UtcNow);

            foreach (var user in users)
            {
                var userCategories = categories.Where(c => c.UserId == user.Id).ToList();
                var taskCount = 2; // 2 tareas por usuario

                for (int i = 0; i < taskCount; i++)
                {
                    var category = userCategories[i % userCategories.Count];
                    var task = taskFaker.Clone()
                        .RuleFor(t => t.UserId, _ => user.Id)
                        .RuleFor(t => t.CategoryId, _ => category.Id)
                        .Generate();

                    taskItems.Add(task);
                }
            }

            return Tuple.Create(users.ToArray(), categories.ToArray(), taskItems.ToArray());
        }

    }
}