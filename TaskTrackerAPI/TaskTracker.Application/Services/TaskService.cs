using AutoMapper;
using TaskTracker.Application;
using TaskTracker.Domain;
using TaskTracker.Domain.Entities;
using TaskTracker.Infrastructure;

namespace TaskTracker.Application
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskCategorizer _categorizer;
        private readonly IMapper _mapper;

        public TaskService(IUnitOfWork unitOfWork, ITaskCategorizer categorizer, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _categorizer = categorizer;
            _mapper = mapper;
        }

        public async Task<List<TaskDto>> GetAllAsync()
        {
            try
            {
                var tasks = await _unitOfWork.Tasks.GetAllAsync();
                return _mapper.Map<List<TaskDto>>(tasks);
            }
            catch
            {
                throw new ApplicationException("Unable to fetch tasks.");
            }
        }

        public async Task<TaskDto?> GetByIdAsync(int id)
        {
            try
            {
                var task = await _unitOfWork.Tasks.GetByIdAsync(id);
                return task == null ? null : _mapper.Map<TaskDto>(task);
            }
            catch
            {
                throw new ApplicationException("Unable to fetch the task.");
            }
        }

        public async Task<TaskDto> CreateAsync(TaskCreateDto dto)
        {
            try
            {
                var task = _mapper.Map<TaskItem>(dto);
                task.CreatedAt = DateTime.UtcNow;
                task.Category = await _categorizer.CategorizeAsync(dto.Description);

                await _unitOfWork.Tasks.AddAsync(task);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<TaskDto>(task);
            }
            catch
            {
                throw new ApplicationException("Failed to create task.");
            }
        }

        public async Task<PagedResult<TaskDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            try
            {
                var totalCount = await _unitOfWork.Tasks.CountAsync();
                var tasks = await _unitOfWork.Tasks.GetPagedAsync(pageNumber, pageSize);
                var items = _mapper.Map<List<TaskDto>>(tasks);

                return new PagedResult<TaskDto>
                {
                    Items = items,
                    TotalCount = totalCount
                };
            }
            catch
            {
                throw new ApplicationException("Unable to fetch data.");
            }
        }

        public async Task<bool> UpdateAsync(int id, TaskUpdateDto dto)
        {
            try
            {
                var task = await _unitOfWork.Tasks.GetByIdAsync(id);
                if (task == null) return false;

                _mapper.Map(dto, task);
                task.Category = await _categorizer.CategorizeAsync(dto.Description);

                await _unitOfWork.Tasks.UpdateAsync(task);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch
            {
                throw new ApplicationException("Failed to update task.");
            }
        }

        public async Task<bool> UpdateStatusAsync(int id, bool status)
        {
            try
            {
                var task = await _unitOfWork.Tasks.GetByIdAsync(id);
                if (task == null) return false;

                task.IsCompleted = status;

                await _unitOfWork.Tasks.UpdateAsync(task);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch
            {
                throw new ApplicationException("Failed to update task.");
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var task = await _unitOfWork.Tasks.GetByIdAsync(id);
                if (task == null) return false;

                await _unitOfWork.Tasks.DeleteAsync(task);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch
            {
                throw new ApplicationException("Failed to delete task.");
            }
        }
    }
}
