
namespace USLabs.TaskManager.Shared.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public UserDTO User { get; set; } = new();
    }
}