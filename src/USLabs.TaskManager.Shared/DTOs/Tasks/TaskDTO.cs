using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using USLabs.TaskManager.Shared.DTOs.Auth;
using USLabs.TaskManager.Shared.Enums;

namespace USLabs.TaskManager.Shared.DTOs.Tasks
{
    public class TaskDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskStatusU Status { get; set; }
        public Priority Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        
        // Navigation properties
        public Guid UserId { get; set; }
        public UserDTO? User { get; set; }
        
        public Guid CategoryId { get; set; }
        public CategoryDTO? Category { get; set; }
    }
}