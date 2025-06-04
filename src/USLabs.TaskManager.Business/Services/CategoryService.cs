using AutoMapper;
using USLabs.TaskManager.Business.Services.Interfaces;
using USLabs.TaskManager.Data.Entities;
using USLabs.TaskManager.Data.Repositories.Interfaces;
using USLabs.TaskManager.Shared.Common;
using USLabs.TaskManager.Shared.DTOs.Categories;

namespace USLabs.TaskManager.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<CategoryDTO>>> GetAllCategoriesAsync(Guid userId)
        {
            try
            {
                var categories = await _categoryRepository.GetCategoriesByUserIdAsync(userId);
                var categoryDtos = _mapper.Map<IEnumerable<CategoryDTO>>(categories);
                
                return ApiResponse<IEnumerable<CategoryDTO>>.SuccessResponse(categoryDtos, "Categories retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<CategoryDTO>>.ErrorResponse($"Error retrieving categories: {ex.Message}");
            }
        }

        public async Task<ApiResponse<CategoryDTO>> GetCategoryByIdAsync(Guid id, Guid userId)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(id);
                
                if (category == null)
                {
                    return ApiResponse<CategoryDTO>.ErrorResponse("Category not found");
                }

                // Verify category belongs to user
                if (category.UserId != userId)
                {
                    return ApiResponse<CategoryDTO>.ErrorResponse("Category not found");
                }

                var categoryDto = _mapper.Map<CategoryDTO>(category);
                return ApiResponse<CategoryDTO>.SuccessResponse(categoryDto, "Category retrieved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CategoryDTO>.ErrorResponse($"Error retrieving category: {ex.Message}");
            }
        }

        public async Task<ApiResponse<CategoryDTO>> CreateCategoryAsync(CreateCategoryDTO createCategoryDto, Guid userId)
        {
            try
            {
                // Check if category name already exists for this user
                var existingCategory = await _categoryRepository.ExistsCategoryByNameAndUserIdAsync(createCategoryDto.Name, userId);
                if (existingCategory)
                {
                    return ApiResponse<CategoryDTO>.ErrorResponse("A category with this name already exists");
                }

                // Map DTO to entity
                var category = _mapper.Map<Category>(createCategoryDto);
                category.UserId = userId;

                // Create category
                var createdCategory = await _categoryRepository.CreateCategoryAsync(category);
                var categoryDto = _mapper.Map<CategoryDTO>(createdCategory);

                return ApiResponse<CategoryDTO>.SuccessResponse(categoryDto, "Category created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CategoryDTO>.ErrorResponse($"Error creating category: {ex.Message}");
            }
        }

        public async Task<ApiResponse<CategoryDTO>> UpdateCategoryAsync(Guid id, UpdateCategoryDTO updateCategoryDto, Guid userId)
        {
            try
            {
                // Check if category exists
                var existingCategory = await _categoryRepository.GetCategoryByIdAsync(id);
                if (existingCategory == null)
                {
                    return ApiResponse<CategoryDTO>.ErrorResponse("Category not found");
                }

                // Verify category belongs to user
                if (existingCategory.UserId != userId)
                {
                    return ApiResponse<CategoryDTO>.ErrorResponse("Category not found");
                }

                // Check if new name already exists for this user (excluding current category)
                var nameExists = await _categoryRepository.ExistsCategoryByNameAndUserIdAsync(updateCategoryDto.Name, userId);
                if (nameExists && existingCategory.Name != updateCategoryDto.Name)
                {
                    return ApiResponse<CategoryDTO>.ErrorResponse("A category with this name already exists");
                }

                // Update category properties
                existingCategory.Name = updateCategoryDto.Name;
                existingCategory.Description = updateCategoryDto.Description;
                existingCategory.Color = updateCategoryDto.Color;

                // Update category
                var updatedCategory = await _categoryRepository.UpdateCategoryAsync(existingCategory);
                var categoryDto = _mapper.Map<CategoryDTO>(updatedCategory);

                return ApiResponse<CategoryDTO>.SuccessResponse(categoryDto, "Category updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CategoryDTO>.ErrorResponse($"Error updating category: {ex.Message}");
            }
        }

        // public async Task<ApiResponse> DeleteCategoryAsync(Guid id, Guid userId)
        // {
        //     try
        //     {
        //         // Check if category exists
        //         var category = await _categoryRepository.GetCategoryByIdAsync(id);
        //         if (category == null)
        //         {
        //             return ApiResponse.ErrorResponse("Category not found");
        //         }

        //         // Verify category belongs to user
        //         if (category.UserId != userId)
        //         {
        //             return ApiResponse.ErrorResponse("Category not found");
        //         }

        //         // Check if category has tasks
        //         var taskCount = await _categoryRepository.GetTaskCountAsync(id);
        //         if (taskCount > 0)
        //         {
        //             return ApiResponse.ErrorResponse("Cannot delete category that contains tasks");
        //         }

        //         // Delete category
        //         var deleted = await _categoryRepository.DeleteCategoryAsync(id);
        //         if (!deleted)
        //         {
        //             return ApiResponse.ErrorResponse("Failed to delete category");
        //         }

        //         return ApiResponse.SuccessResponse("Category deleted successfully");
        //     }
        //     catch (Exception ex)
        //     {
        //         return ApiResponse.ErrorResponse($"Error deleting category: {ex.Message}");
        //     }
        // }

        public async Task<bool> CategoryExistsAsync(Guid id)
        {
            try
            {
                return await _categoryRepository.ExistsCategoryAsync(id);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CategoryBelongsToUserAsync(Guid categoryId, Guid userId)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(categoryId);
                return category != null && category.UserId == userId;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}