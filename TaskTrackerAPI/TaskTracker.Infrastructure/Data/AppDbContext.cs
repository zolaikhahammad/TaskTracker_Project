using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure.Configurations;

namespace TaskTracker.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TaskItem> Tasks => Set<TaskItem>();
        public DbSet<TaskPriorityItem> TaskPriorities => Set<TaskPriorityItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItem>()
                .HasKey(t => t.TaskId);

            modelBuilder.ApplyConfiguration(new TaskPriorityItemConfiguration());

           
        }

    }
}