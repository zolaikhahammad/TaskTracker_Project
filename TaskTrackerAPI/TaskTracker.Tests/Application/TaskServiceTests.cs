using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using TaskTracker.Application;
using TaskTracker.Domain;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure;

namespace TaskTracker.Tests.Application
{
    public class TaskServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ITaskRepository> _taskRepoMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly ITaskCategorizer _categorizer;
        private readonly TaskService _service;

        public TaskServiceTests(IConfiguration configuration)
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _taskRepoMock = new Mock<ITaskRepository>();
            _unitOfWorkMock.Setup(u => u.Tasks).Returns(_taskRepoMock.Object);

            _mapperMock = new Mock<IMapper>();
            _categorizer = new KeywordTaskCategorizer(configuration);
            _service = new TaskService(_unitOfWorkMock.Object, _categorizer, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnMappedTaskDtos()
        {
            var tasks = new List<TaskItem> { new TaskItem { TaskId = 1, Title = "Test" } };
            var taskDtos = new List<TaskDto> { new TaskDto { TaskId = 1, Title = "Test" } };

            _taskRepoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(tasks);
            _mapperMock.Setup(m => m.Map<List<TaskDto>>(tasks)).Returns(taskDtos);

            var result = await _service.GetAllAsync();

            result.Should().BeEquivalentTo(taskDtos);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMappedDto_WhenTaskExists()
        {
            var task = new TaskItem { TaskId = 1, Title = "Test" };
            var dto = new TaskDto { TaskId = 1, Title = "Test" };

            _taskRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(task);
            _mapperMock.Setup(m => m.Map<TaskDto>(task)).Returns(dto);

            var result = await _service.GetByIdAsync(1);

            result.Should().BeEquivalentTo(dto);
        }

        [Fact]
        public async Task CreateAsync_ShouldMapAndSaveTask()
        {
            var dto = new TaskCreateDto { Title = "Test", Description = "Test", DueDate = DateTime.UtcNow, TaskPriorityId = 1 };
            var task = new TaskItem { Title = "Test" };
            var mappedDto = new TaskDto { Title = "Test" };

            _mapperMock.Setup(m => m.Map<TaskItem>(dto)).Returns(task);
            _mapperMock.Setup(m => m.Map<TaskDto>(task)).Returns(mappedDto);

            var result = await _service.CreateAsync(dto);

            _taskRepoMock.Verify(r => r.AddAsync(task), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
            result.Should().BeEquivalentTo(mappedDto);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnFalse_WhenTaskNotFound()
        {
            _taskRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((TaskItem?)null);

            var result = await _service.UpdateAsync(1, new TaskUpdateDto());

            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenTaskNotFound()
        {
            _taskRepoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((TaskItem?)null);

            var result = await _service.DeleteAsync(1);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetAllAsyncPaged_ShouldReturnPagedResult()
        {
            var page = 1;
            var size = 2;
            var total = 10;
            var tasks = new List<TaskItem> { new TaskItem { TaskId = 1 }, new TaskItem { TaskId = 2 } };
            var taskDtos = new List<TaskDto> { new TaskDto { TaskId = 1 }, new TaskDto { TaskId = 2 } };

            _taskRepoMock.Setup(r => r.CountAsync()).ReturnsAsync(total);
            _taskRepoMock.Setup(r => r.GetPagedAsync(page, size)).ReturnsAsync(tasks);
            _mapperMock.Setup(m => m.Map<List<TaskDto>>(tasks)).Returns(taskDtos);

            var result = await _service.GetAllAsync(page, size);

            result.TotalCount.Should().Be(total);
            result.Items.Should().BeEquivalentTo(taskDtos);
        }
    }
}
