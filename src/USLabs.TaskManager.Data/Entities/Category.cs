using System.ComponentModel.DataAnnotations;

namespace USLabs.TaskManager.Data.Entities
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
        [MaxLength(250)]
        public string? Description { get; set; }
        public string? Color { get; set; } = "#007bff";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Foreign key for User
        [Required]
        public Guid UserId { get; set; }

        // Navigation property for User
        public virtual User? User { get; set; }
    }
}