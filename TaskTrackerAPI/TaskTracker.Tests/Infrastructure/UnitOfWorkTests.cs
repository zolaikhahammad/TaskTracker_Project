using Xunit;
using Moq;
using TaskTracker.Infrastructure;
using TaskTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace TaskTracker.Tests.Infrastructure
{
    public class UnitOfWorkTests
    {
        [Fact]
        public async Task SaveChangesAsync_ShouldCall_DbContext_SaveChangesAsync()
        {
            // Arrange
            var dbContextMock = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            dbContextMock.Setup(db => db.SaveChangesAsync(default)).ReturnsAsync(1).Verifiable();

            var unitOfWork = new UnitOfWork(dbContextMock.Object);

            // Act
            await unitOfWork.SaveChangesAsync();

            // Assert
            dbContextMock.Verify(db => db.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public void Tasks_ShouldReturn_TaskRepository_Instance()
        {
            // Arrange
            var dbContextMock = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            var unitOfWork = new UnitOfWork(dbContextMock.Object);

            // Act
            var repo = unitOfWork.Tasks;

            // Assert
            Assert.NotNull(repo);
            Assert.IsType<TaskRepository>(repo);
        }
    }
}
