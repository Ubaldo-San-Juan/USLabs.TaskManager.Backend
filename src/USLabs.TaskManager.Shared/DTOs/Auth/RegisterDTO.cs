using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace USLabs.TaskManager.Shared.DTOs.Auth
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid format Email")]
        [MaxLength(250, ErrorMessage = "Maximun 250 caracters for Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Confirm password is required")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required")]
        [MaxLength(100, ErrorMessage = "First name cannot exceed 100 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(100, ErrorMessage = "Last name cannot exceed 100 characters")]
        public string LastName { get; set; } = string.Empty;
    }
}