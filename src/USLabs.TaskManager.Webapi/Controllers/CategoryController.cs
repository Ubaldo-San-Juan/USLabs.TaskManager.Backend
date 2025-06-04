using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using USLabs.TaskManager.Business.Services.Interfaces;
using USLabs.TaskManager.Shared.Common;
using USLabs.TaskManager.Shared.DTOs.Categories;

namespace USLabs.TaskManager.Webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<CategoryDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<IEnumerable<CategoryDTO>>>> GetCategories()
        {
            var userId = GetCurrentUserId();
            var result = await _categoryService.GetAllCategoriesAsync(userId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<CategoryDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<CategoryDTO>>> GetCategory(Guid id)
        {
            var userId = GetCurrentUserId();

            // Check if category exists and belongs to user
            if (!await _categoryService.CategoryExistsAsync(id))
            {
                return NotFound(ApiResponse<CategoryDTO>.ErrorResponse("Category not found"));
            }

            if (!await _categoryService.CategoryBelongsToUserAsync(id, userId))
            {
                return Forbid();
            }

            var result = await _categoryService.GetCategoryByIdAsync(id, userId);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }


        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CategoryDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<CategoryDTO>>> CreateCategory([FromBody] CreateCategoryDTO createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var result = await _categoryService.CreateCategoryAsync(createCategoryDto, userId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(nameof(GetCategory), new { id = result.Data!.Id }, result);
        }


        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<CategoryDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<CategoryDTO>>> UpdateCategory(Guid id, [FromBody] UpdateCategoryDTO updateCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();

            // Check if category exists and belongs to user
            if (!await _categoryService.CategoryExistsAsync(id))
            {
                return NotFound(ApiResponse<CategoryDTO>.ErrorResponse("Category not found"));
            }

            if (!await _categoryService.CategoryBelongsToUserAsync(id, userId))
            {
                return Forbid();
            }

            var result = await _categoryService.UpdateCategoryAsync(id, updateCategoryDto, userId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
        
                private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                           ?? User.FindFirst("sub")?.Value 
                           ?? User.FindFirst("userId")?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }
            
            return userId;
        }
    }
}