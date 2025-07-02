using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Infrastructure.Configurations
{
    public class TaskPriorityItemConfiguration : IEntityTypeConfiguration<TaskPriorityItem>
    {
        public void Configure(EntityTypeBuilder<TaskPriorityItem> builder)
        {
            builder.HasKey(tp => tp.TaskPriorityId);

            builder.HasMany(t => t.Tasks)
                 .WithOne(t => t.TaskPriority)
                  .HasForeignKey(t => t.TaskPriorityId)
                  .OnDelete(DeleteBehavior.Restrict);


            builder.HasData(
                new TaskPriorityItem { TaskPriorityId = 1, Name = "Low" },
                new TaskPriorityItem { TaskPriorityId = 2, Name = "Medium" },
                new TaskPriorityItem { TaskPriorityId = 3, Name = "High" }
            );
        }
    }
}
