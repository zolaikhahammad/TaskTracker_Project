using TaskTracker.Domain;
using TaskTracker.Domain.Entities;

namespace TaskTracker.Application
{
    public interface ITaskService
    {
        Task<List<TaskDto>> GetAllAsync();
        Task<TaskDto?> GetByIdAsync(int id);
        Task<PagedResult<TaskDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<TaskDto> CreateAsync(TaskCreateDto dto);
        Task<bool> UpdateAsync(int id, TaskUpdateDto dto);
        Task<bool> UpdateStatusAsync(int id, bool status);
        Task<bool> DeleteAsync(int id);
    }

}
