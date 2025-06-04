using Microsoft.EntityFrameworkCore;
using USLabs.TaskManager.Data.Context;
using USLabs.TaskManager.Data.Entities;
using USLabs.TaskManager.Data.Repositories.Interfaces;
using USLabs.TaskManager.Shared.Enums;

namespace USLabs.TaskManager.Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagerContext _context;

        public TaskRepository(TaskManagerContext context)
        {
            _context = context;
        }

        public async Task<TaskItem?> GetTaskItemByIdAsync(Guid id)
        {
            return await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<TaskItem>> GetAllTaskItemsAsync()
        {
            return await _context.TaskItems.ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetTaskItemsByUserId(Guid userId)
        {
            return await _context.TaskItems
                .Include(t => t.Category)
                .Where(t => t.UserId == userId)
                .OrderBy(t => t.DueDate)
                .ThenBy(t => t.Priority)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetTaskItemsByCategoryId(Guid categoryId)
        {
            return await _context.TaskItems
                .Include(t => t.User)
                .Include(t => t.Category)
                .Where(t => t.CategoryId == categoryId)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetTaskByStatusAsync(TaskStatusU status)
        {
            return await _context.TaskItems
                .Include(t => t.User)
                .Include(t => t.Category)
                .Where(t => t.Status == status)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetTaskByPriorityAsync(Priority priority)
        {
            return await _context.TaskItems
                .Include(t => t.User)
                .Include(t => t.Category)
                .Where(t => t.Priority == priority)
                .OrderBy(t => t.DueDate)
                .ToListAsync();
        }

        public async Task<TaskItem> CreateTaskAsync(TaskItem taskItem)
        {
            _context.Add(taskItem);
            await _context.SaveChangesAsync();
            return taskItem;
        }

        public async Task<TaskItem> UpdateTaskAsync(TaskItem taskItem)
        {
            _context.Update(taskItem);
            await _context.SaveChangesAsync();
            return taskItem;
        }

        public async Task<bool> DeleteTaskAsync(Guid id)
        {
            var task = await _context.TaskItems.FindAsync(id);
            if (task == null)
            {
                return false;
            }

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsTaskAsync(Guid id)
        {
            return await _context.TaskItems.AnyAsync(t => t.Id == id);
        }
    }
}