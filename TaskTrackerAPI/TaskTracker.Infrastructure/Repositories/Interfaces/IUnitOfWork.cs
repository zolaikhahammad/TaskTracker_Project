namespace TaskTracker.Infrastructure
{ 
    public interface IUnitOfWork
    {
        ITaskRepository Tasks { get; }
        Task SaveChangesAsync();
    }
}
