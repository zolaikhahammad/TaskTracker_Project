using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure;
using Xunit;

namespace TaskTracker.Tests.Infrastructure
{
    public class TaskRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly TaskRepository _repository;

        public TaskRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _context.Database.EnsureCreated();

            _repository = new TaskRepository(_context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddTask()
        {
            var task = new TaskItem { Title = "Test", Description = "Desc", DueDate = DateTime.UtcNow, TaskPriorityId = 1 };

            await _repository.AddAsync(task);
            await _context.SaveChangesAsync();

            var savedTask = await _context.Tasks.FirstOrDefaultAsync();
            savedTask.Should().NotBeNull();
            savedTask!.Title.Should().Be("Test");
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnTasks()
        {
            _context.Tasks.Add(new TaskItem { Title = "One", Description = "Desc", DueDate = DateTime.UtcNow });
            _context.Tasks.Add(new TaskItem { Title = "Two", Description = "Desc", DueDate = DateTime.UtcNow });
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllAsync();

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCorrectTask()
        {
            var task = new TaskItem { Title = "FindMe", Description = "Desc", DueDate = DateTime.UtcNow };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var found = await _repository.GetByIdAsync(task.TaskId);

            found.Should().NotBeNull();
            found!.Title.Should().Be("FindMe");
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateTask()
        {
            var task = new TaskItem { Title = "Old", Description = "Desc", DueDate = DateTime.UtcNow };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            task.Title = "Updated";
            await _repository.UpdateAsync(task);
            await _context.SaveChangesAsync();

            var updated = await _context.Tasks.FindAsync(task.TaskId);
            updated!.Title.Should().Be("Updated");
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveTask()
        {
            var task = new TaskItem { Title = "Delete", Description = "Desc", DueDate = DateTime.UtcNow };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            await _repository.DeleteAsync(task);
            await _context.SaveChangesAsync();

            var deleted = await _context.Tasks.FindAsync(task.TaskId);
            deleted.Should().BeNull();
        }

        [Fact]
        public async Task GetPagedAsync_ShouldReturnCorrectPage()
        {
            for (int i = 1; i <= 10; i++)
            {
                _context.Tasks.Add(new TaskItem { Title = $"Task {i}", Description = "Desc", DueDate = DateTime.UtcNow.AddDays(i) });
            }
            await _context.SaveChangesAsync();

            var result = await _repository.GetPagedAsync(2, 3);

            result.Should().HaveCount(3);
            result.First().Title.Should().Be("Task 7");
        }

        [Fact]
        public async Task CountAsync_ShouldReturnCorrectCount()
        {
            _context.Tasks.Add(new TaskItem { Title = "A", Description = "Desc", DueDate = DateTime.UtcNow });
            _context.Tasks.Add(new TaskItem { Title = "B", Description = "Desc", DueDate = DateTime.UtcNow });
            await _context.SaveChangesAsync();

            var count = await _repository.CountAsync();

            count.Should().Be(2);
        }
    }
}
