using USLabs.TaskManager.Shared.Common;
using USLabs.TaskManager.Shared.DTOs.Categories;

namespace USLabs.TaskManager.Business.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ApiResponse<IEnumerable<CategoryDTO>>> GetAllCategoriesAsync(Guid userId);
        Task<ApiResponse<CategoryDTO>> GetCategoryByIdAsync(Guid id, Guid userId);
        Task<ApiResponse<CategoryDTO>> CreateCategoryAsync(CreateCategoryDTO createCategoryDto, Guid userId);
        Task<ApiResponse<CategoryDTO>> UpdateCategoryAsync(Guid id, UpdateCategoryDTO updateCategoryDto, Guid userId);
        // Task<ApiResponse>> DeleteCategoryAsync(Guid id, Guid userId);
        Task<bool> CategoryExistsAsync(Guid id);
        Task<bool> CategoryBelongsToUserAsync(Guid categoryId, Guid userId);
    }
}