using Microsoft.EntityFrameworkCore;
using USLabs.TaskManager.Data.Context;
using USLabs.TaskManager.Data.Entities;
using USLabs.TaskManager.Data.Repositories.Interfaces;

namespace USLabs.TaskManager.Data.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly TaskManagerContext _context;
        public CategoryRepository(TaskManagerContext context)
        {
            _context = context;
        }

        public async Task<Category?> GetCategoryByIdAsync(Guid id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category?> GetCategoryByNameAsync(string name)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesByUserIdAsync(Guid userId)
        {
            return await _context.Categories
                .Include(c => c.TaskItems)
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return false;
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsCategoryAsync(Guid id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> ExistsCategoryByNameAndUserIdAsync(string name, Guid userId)
        {
            return await _context.Categories.AnyAsync(c => c.Name!.ToLower() == name.ToLower() && c.UserId == userId);
        }

        public async Task<int> GetTaskCountAsync(Guid categoryId)
        {
            return await _context.TaskItems.CountAsync(t => t.CategoryId == categoryId);
        }
    }
}