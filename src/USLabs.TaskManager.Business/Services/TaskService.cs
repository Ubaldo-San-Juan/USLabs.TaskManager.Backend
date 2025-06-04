using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using USLabs.TaskManager.Business.Services.Interfaces;
using USLabs.TaskManager.Data.Entities;
using USLabs.TaskManager.Data.Repositories.Interfaces;
using USLabs.TaskManager.Shared.Common;
using USLabs.TaskManager.Shared.DTOs.Tasks;
using USLabs.TaskManager.Shared.Enums;

namespace USLabs.TaskManager.Business.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public TaskService(ITaskRepository taskRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _taskRepository = taskRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PaginatedResult<TaskDTO>>> GetAllTasksAsync(Guid userId, TaskFilterDTO filter)
        {
            try
            {
                // Get all tasks for user
                var allTasks = await _taskRepository.GetTaskItemsByUserId(userId);
                var taskList = allTasks.ToList();

                // Apply filters
                if (filter.Status.HasValue)
                {
                    taskList = taskList.Where(t => t.Status == filter.Status.Value).ToList();
                }

                if (filter.Priority.HasValue)
                {
                    taskList = taskList.Where(t => t.Priority == filter.Priority.Value).ToList();
                }

                if (filter.CategoryId.HasValue)
                {
                    taskList = taskList.Where(t => t.CategoryId == filter.CategoryId.Value).ToList();
                }

                if (filter.DueDateFrom.HasValue)
                {
                    taskList = taskList.Where(t => t.DueDate >= filter.DueDateFrom.Value).ToList();
                }

                if (filter.DueDateTo.HasValue)
                {
                    taskList = taskList.Where(t => t.DueDate <= filter.DueDateTo.Value).ToList();
                }

                if (!string.IsNullOrEmpty(filter.SearchTerm))
                {
                    taskList = taskList.Where(t =>
                        t.Title!.Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        t.Description!.Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // Apply sorting
                taskList = filter.SortBy.ToLower() switch
                {
                    "title" => filter.SortDescending
                        ? taskList.OrderByDescending(t => t.Title).ToList()
                        : taskList.OrderBy(t => t.Title).ToList(),
                    "priority" => filter.SortDescending
                        ? taskList.OrderByDescending(t => t.Priority).ToList()
                        : taskList.OrderBy(t => t.Priority).ToList(),
                    "status" => filter.SortDescending
                        ? taskList.OrderByDescending(t => t.Status).ToList()
                        : taskList.OrderBy(t => t.Status).ToList(),
                    "createdat" => filter.SortDescending
                        ? taskList.OrderByDescending(t => t.CreatedAt).ToList()
                        : taskList.OrderBy(t => t.CreatedAt).ToList(),
                    _ => filter.SortDescending
                        ? taskList.OrderByDescending(t => t.DueDate).ToList()
                        : taskList.OrderBy(t => t.DueDate).ToList()
                };

                // Apply pagination
                var totalCount = taskList.Count;
                var pagedTasks = taskList
                    .Skip((filter.Page - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToList();

                // Map to DTOs
                var taskDtos = _mapper.Map<List<TaskDTO>>(pagedTasks);

                var paginatedResult = PaginatedResult<TaskDTO>.Create(taskDtos, totalCount, filter.Page, filter.PageSize);

                return ApiResponse<PaginatedResult<TaskDTO>>.SuccessResponse(paginatedResult, "Tasks retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<PaginatedResult<TaskDTO>>.ErrorResponse($"Error retrieving tasks: {ex.Message}");
            }
        }


        public async Task<ApiResponse<TaskDTO>> GetTaskByIdAsync(Guid id, Guid userId)
        {
            try
            {
                var task = await _taskRepository.GetTaskItemByIdAsync(id);

                if (task == null)
                {
                    return ApiResponse<TaskDTO>.ErrorResponse("Task not found");
                }

                // Verify task belongs to user
                if (task.UserId != userId)
                {
                    return ApiResponse<TaskDTO>.ErrorResponse("Task not found");
                }

                var taskDto = _mapper.Map<TaskDTO>(task);
                return ApiResponse<TaskDTO>.SuccessResponse(taskDto, "Task retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<TaskDTO>.ErrorResponse($"Error retrieving task: {ex.Message}");
            }
        }

        public async Task<ApiResponse<TaskDTO>> CreateTaskAsync(CreateTaskDTO createTaskDto, Guid userId)
        {
            try
            {
                // Verify category exists and belongs to user
                var category = await _categoryRepository.GetCategoryByIdAsync(createTaskDto.CategoryId);
                if (category == null || category.UserId != userId)
                {
                    return ApiResponse<TaskDTO>.ErrorResponse("Invalid category");
                }

                // Map DTO to entity
                var task = _mapper.Map<TaskItem>(createTaskDto);
                task.UserId = userId;

                // Create task
                var createdTask = await _taskRepository.CreateTaskAsync(task);
                var taskDto = _mapper.Map<TaskDTO>(createdTask);

                return ApiResponse<TaskDTO>.SuccessResponse(taskDto, "Task created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<TaskDTO>.ErrorResponse($"Error creating task: {ex.Message}");
            }
        }


        public async Task<ApiResponse<TaskDTO>> UpdateTaskAsync(Guid id, UpdateTaskDTO updateTaskDto, Guid userId)
        {
            try
            {
                // Check if task exists
                var existingTask = await _taskRepository.GetTaskItemByIdAsync(id);
                if (existingTask == null)
                {
                    return ApiResponse<TaskDTO>.ErrorResponse("Task not found");
                }

                // Verify task belongs to user
                if (existingTask.UserId != userId)
                {
                    return ApiResponse<TaskDTO>.ErrorResponse("Task not found");
                }

                // Verify category exists and belongs to user
                var category = await _categoryRepository.GetCategoryByIdAsync(updateTaskDto.CategoryId);
                if (category == null || category.UserId != userId)
                {
                    return ApiResponse<TaskDTO>.ErrorResponse("Invalid category");
                }

                // Update task properties
                existingTask.Title = updateTaskDto.Title;
                existingTask.Description = updateTaskDto.Description;
                existingTask.Status = updateTaskDto.Status;
                existingTask.Priority = updateTaskDto.Priority;
                existingTask.DueDate = updateTaskDto.DueDate;
                existingTask.CategoryId = updateTaskDto.CategoryId;
                existingTask.UpdatedAt = DateTime.UtcNow;

                // Set completed date if status is completed
                if (updateTaskDto.Status == TaskStatusU.Completed && existingTask.CompletedAt == null)
                {
                    existingTask.CompletedAt = DateTime.UtcNow;
                }
                else if (updateTaskDto.Status != TaskStatusU.Completed)
                {
                    existingTask.CompletedAt = null;
                }

                // Update task
                var updatedTask = await _taskRepository.UpdateTaskAsync(existingTask);
                var taskDto = _mapper.Map<TaskDTO>(updatedTask);

                return ApiResponse<TaskDTO>.SuccessResponse(taskDto, "Task updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<TaskDTO>.ErrorResponse($"Error updating task: {ex.Message}");
            }
        }



        // public async Task<ApiResponse> DeleteTaskAsync(Guid id, Guid userId)
        // {
        //     try
        //     {
        //         // Check if task exists
        //         var task = await _taskRepository.GetTaskItemByIdAsync(id);
        //         if (task == null)
        //         {
        //             return ApiResponse.ErrorResponse("Task not found");
        //         }

        //         // Verify task belongs to user
        //         if (task.UserId != userId)
        //         {
        //             return ApiResponse.ErrorResponse("Task not found");
        //         }

        //         // Delete task
        //         var deleted = await _taskRepository.DeleteTaskAsync(id);
        //         if (!deleted)
        //         {
        //             return ApiResponse.ErrorResponse("Failed to delete task");
        //         }

        //         return ApiResponse.SuccessResponse("Task deleted successfully");
        //     }
        //     catch (Exception ex)
        //     {
        //         return ApiResponse.ErrorResponse($"Error deleting task: {ex.Message}");
        //     }
        // }

        public async Task<ApiResponse<TaskDTO>> UpdateTaskStatusAsync(Guid id, TaskStatusU status, Guid userId)
        {
            try
            {
                // Check if task exists
                var existingTask = await _taskRepository.GetTaskItemByIdAsync(id);
                if (existingTask == null)
                {
                    return ApiResponse<TaskDTO>.ErrorResponse("Task not found");
                }

                // Verify task belongs to user
                if (existingTask.UserId != userId)
                {
                    return ApiResponse<TaskDTO>.ErrorResponse("Task not found");
                }

                // Update status
                existingTask.Status = status;
                existingTask.UpdatedAt = DateTime.UtcNow;

                // Set/clear completed date
                if (status == TaskStatusU.Completed && existingTask.CompletedAt == null)
                {
                    existingTask.CompletedAt = DateTime.UtcNow;
                }
                else if (status != TaskStatusU.Completed)
                {
                    existingTask.CompletedAt = null;
                }

                // Update task
                var updatedTask = await _taskRepository.UpdateTaskAsync(existingTask);
                var taskDto = _mapper.Map<TaskDTO>(updatedTask);

                return ApiResponse<TaskDTO>.SuccessResponse(taskDto, "Task status updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<TaskDTO>.ErrorResponse($"Error updating task status: {ex.Message}");
            }
        }

        public async Task<bool> TaskExistsAsync(Guid id)
        {
            try
            {
                return await _taskRepository.ExistsTaskAsync(id);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> TaskBelongsToUserAsync(Guid taskId, Guid userId)
        {
            try
            {
                var task = await _taskRepository.GetTaskItemByIdAsync(taskId);
                return task != null && task.UserId == userId;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
    }
}