using USLabs.TaskManager.Shared.Common;
using USLabs.TaskManager.Shared.DTOs.Tasks;
using USLabs.TaskManager.Shared.Enums;

namespace USLabs.TaskManager.Business.Services.Interfaces
{
    public interface ITaskService
    {
        Task<ApiResponse<PaginatedResult<TaskDTO>>> GetAllTasksAsync(Guid userId, TaskFilterDTO filter);
        Task<ApiResponse<TaskDTO>> GetTaskByIdAsync(Guid id, Guid userId);
        Task<ApiResponse<TaskDTO>> CreateTaskAsync(CreateTaskDTO createTaskDto, Guid userId);
        Task<ApiResponse<TaskDTO>> UpdateTaskAsync(Guid id, UpdateTaskDTO updateTaskDto, Guid userId);
        // Task<ApiResponse> DeleteTaskAsync(Guid id, Guid userId);
        Task<ApiResponse<TaskDTO>> UpdateTaskStatusAsync(Guid id, TaskStatusU status, Guid userId);
        Task<bool> TaskExistsAsync(Guid id);
        Task<bool> TaskBelongsToUserAsync(Guid taskId, Guid userId);
    }
}