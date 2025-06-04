using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using USLabs.TaskManager.Business.Services.Interfaces;
using USLabs.TaskManager.Shared.Common;
using USLabs.TaskManager.Shared.DTOs.Tasks;
using USLabs.TaskManager.Shared.Enums;

namespace USLabs.TaskManager.Webapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ICategoryService _categoryService;

        public TaskController(ITaskService taskService, ICategoryService categoryService)
        {
            _taskService = taskService;
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PaginatedResult<TaskDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<PaginatedResult<TaskDTO>>>> GetTasks([FromQuery] TaskFilterDTO filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();
            var result = await _taskService.GetAllTasksAsync(userId, filter);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<TaskDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<TaskDTO>>> GetTask(Guid id)
        {
            var userId = GetCurrentUserId();

            // Check if task exists and belongs to user
            if (!await _taskService.TaskExistsAsync(id))
            {
                return NotFound(ApiResponse<TaskDTO>.ErrorResponse("Task not found"));
            }

            if (!await _taskService.TaskBelongsToUserAsync(id, userId))
            {
                return Forbid();
            }

            var result = await _taskService.GetTaskByIdAsync(id, userId);

            if (!result.Success)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<TaskDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<TaskDTO>>> CreateTask([FromBody] CreateTaskDTO createTaskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();

            // Validate category exists and belongs to user
            if (!await _categoryService.CategoryExistsAsync(createTaskDto.CategoryId))
            {
                return BadRequest(ApiResponse<TaskDTO>.ErrorResponse("Category not found"));
            }

            if (!await _categoryService.CategoryBelongsToUserAsync(createTaskDto.CategoryId, userId))
            {
                return BadRequest(ApiResponse<TaskDTO>.ErrorResponse("Category does not belong to current user"));
            }

            var result = await _taskService.CreateTaskAsync(createTaskDto, userId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return CreatedAtAction(nameof(GetTask), new { id = result.Data!.Id }, result);
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<TaskDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<TaskDTO>>> UpdateTask(Guid id, [FromBody] UpdateTaskDTO updateTaskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();

            // Check if task exists and belongs to user
            if (!await _taskService.TaskExistsAsync(id))
            {
                return NotFound(ApiResponse<TaskDTO>.ErrorResponse("Task not found"));
            }

            if (!await _taskService.TaskBelongsToUserAsync(id, userId))
            {
                return Forbid();
            }

            // Validate category exists and belongs to user
            if (!await _categoryService.CategoryExistsAsync(updateTaskDto.CategoryId))
            {
                return BadRequest(ApiResponse<TaskDTO>.ErrorResponse("Category not found"));
            }

            if (!await _categoryService.CategoryBelongsToUserAsync(updateTaskDto.CategoryId, userId))
            {
                return BadRequest(ApiResponse<TaskDTO>.ErrorResponse("Category does not belong to current user"));
            }

            var result = await _taskService.UpdateTaskAsync(id, updateTaskDto, userId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPatch("{id:guid}/status")]
        [ProducesResponseType(typeof(ApiResponse<TaskDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<TaskDTO>>> UpdateTaskStatus(Guid id, [FromBody] TaskStatusUpdateDTO statusUpdate)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetCurrentUserId();

            // Check if task exists and belongs to user
            if (!await _taskService.TaskExistsAsync(id))
            {
                return NotFound(ApiResponse<TaskDTO>.ErrorResponse("Task not found"));
            }

            if (!await _taskService.TaskBelongsToUserAsync(id, userId))
            {
                return Forbid();
            }

            var result = await _taskService.UpdateTaskStatusAsync(id, statusUpdate.Status, userId);

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
        
        public class TaskStatusUpdateDTO
        {
            public TaskStatusU Status { get; set; }
        }
    }
}