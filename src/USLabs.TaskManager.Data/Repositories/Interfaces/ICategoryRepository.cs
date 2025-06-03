using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using USLabs.TaskManager.Data.Entities;

namespace USLabs.TaskManager.Data.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category?> GetCategoryByIdAsync(Guid id);
        Task<Category?> GetCategoryByNameAsync(string name);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<IEnumerable<Category>> GetCategoriesByUserIdAsync(Guid userId);

        Task<Category> CreateCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(Guid id);
        Task<bool> ExistsCategoryAsync(Guid id);
        Task<bool> ExistsCategoryByNameAndUserIdAsync(string categoryName, Guid userId);
        Task<int> GetTaskCountAsync(Guid categoryId);
    }
}