using TaskTracker.Domain.Entities;

namespace TaskTracker.Infrastructure
{
    public interface ITaskRepository
    {
        Task<List<TaskItem>> GetAllAsync();
        Task<TaskItem?> GetByIdAsync(int id);
        Task AddAsync(TaskItem task);
        Task UpdateAsync(TaskItem task);
        Task DeleteAsync(TaskItem task);

        Task<List<TaskItem>> GetPagedAsync(int pageNumber, int pageSize);
        Task<int> CountAsync();
    }
}
