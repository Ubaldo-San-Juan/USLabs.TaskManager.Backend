
namespace USLabs.TaskManager.Shared.DTOs.Tasks
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Color { get; set; } = "#007bff";
        public Guid UserId { get; set; }
        public int TaskCount { get; set; }
    }
}