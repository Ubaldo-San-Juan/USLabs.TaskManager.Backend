using USLabs.TaskManager.Data.Entities;
using USLabs.TaskManager.Shared.Enums;

namespace USLabs.TaskManager.Data.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<TaskItem?> GetTaskItemByIdAsync(Guid id);
        Task<IEnumerable<TaskItem>> GetAllTaskItemsAsync();
        Task<IEnumerable<TaskItem>> GetTaskItemsByUserId(Guid userId);
        Task<IEnumerable<TaskItem>> GetTaskItemsByCategoryId(Guid categoryId);
        Task<IEnumerable<TaskItem>> GetTaskByStatusAsync(TaskStatusU status);
        Task<IEnumerable<TaskItem>> GetTaskByPriorityAsync(Priority priority);

        Task<TaskItem> CreateTaskAsync(TaskItem taskItem);
        Task<TaskItem> UpdateTaskAsync(TaskItem taskItem);
        Task<bool> DeleteTaskAsync(Guid id);
        Task<bool> ExistsTaskAsync(Guid id);
    }
}