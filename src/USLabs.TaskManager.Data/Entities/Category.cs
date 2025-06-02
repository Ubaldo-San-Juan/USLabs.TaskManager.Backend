using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        // Navigation property for TaskItems
        public virtual ICollection<TaskItem>? TaskItems { get; set; } = new List<TaskItem>();
    }
}