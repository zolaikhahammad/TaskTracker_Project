using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entities;


namespace TaskTracker.Infrastructure
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskItem>> GetAllAsync()
        {
           return await _context.Tasks.ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(int id) 
        {
            return await _context.Tasks.FirstOrDefaultAsync(x => x.TaskId == id); 
        }

        public async Task AddAsync(TaskItem task)
        {
            await _context.Tasks.AddAsync(task);
        }

        public async Task UpdateAsync(TaskItem task)
        {
            _context.Tasks.Update(task);
        }

        public async Task DeleteAsync(TaskItem task)
        {
            _context.Tasks.Remove(task);
        }

        public async Task<List<TaskItem>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.Tasks
                .Include(t => t.TaskPriority)
                .OrderByDescending(x => x.DueDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _context.Tasks.CountAsync();
        }
    }
}
