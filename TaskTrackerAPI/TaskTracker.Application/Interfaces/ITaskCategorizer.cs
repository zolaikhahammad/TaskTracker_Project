using TaskTracker.Domain;

namespace TaskTracker.Application
{
    public interface ITaskCategorizer
    {
        Task<string> CategorizeAsync(string description);
    }

}
