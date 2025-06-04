using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using USLabs.TaskManager.Shared.Enums;

namespace USLabs.TaskManager.Shared.DTOs.Tasks
{
    public class TaskFilterDTO
    {
        public TaskStatusU? Status { get; set; }
        public Priority? Priority { get; set; }
        public Guid? CategoryId { get; set; }
        public DateTime? DueDateFrom { get; set; }
        public DateTime? DueDateTo { get; set; }
        public string? SearchTerm { get; set; }
        public int Page { get; set; } = 1;
            [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "DueDate";
        public bool SortDescending { get; set; } = false;
    }
}