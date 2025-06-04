
namespace USLabs.TaskManager.Shared.DTOs.Categories
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Color { get; set; } = "#007bff";
        public DateTime CreatedAt { get; set; }
        
        // Statistics
        public int TaskCount { get; set; }
        public int CompletedTaskCount { get; set; }
        public int PendingTaskCount { get; set; }
        
        // User info
        public Guid UserId { get; set; }
    }
}