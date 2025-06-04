using USLabs.TaskManager.Shared.DTOs.Auth;

namespace USLabs.TaskManager.Business.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO?> LoginAsync(LoginDTO loginDTO);
        Task<AuthResponseDTO?> RegisterAsync(RegisterDTO registerDTO);
        Task<UserDTO?> GetUserByIdAsync(Guid userId);
        Task<bool> ValidateUserAsync(Guid userId);
    }
}