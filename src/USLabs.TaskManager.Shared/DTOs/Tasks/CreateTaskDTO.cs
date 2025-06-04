using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using USLabs.TaskManager.Shared.Enums;

namespace USLabs.TaskManager.Shared.DTOs.Tasks
{
    public class CreateTaskDTO : IValidatableObject
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Priority is required")]
        public Priority Priority { get; set; } = Priority.Medium;

        public DateTime? DueDate { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public Guid CategoryId { get; set; }

        // Validation: DueDate cannot be in the past
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DueDate.HasValue && DueDate.Value.Date < DateTime.Today)
            {
                yield return new ValidationResult(
                    "Due date cannot be in the past",
                    new[] { nameof(DueDate) });
            }
        }
    }
}