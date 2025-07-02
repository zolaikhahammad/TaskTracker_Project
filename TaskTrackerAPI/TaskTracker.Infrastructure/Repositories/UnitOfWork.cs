
namespace TaskTracker.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public ITaskRepository Tasks { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Tasks = new TaskRepository(_context);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
