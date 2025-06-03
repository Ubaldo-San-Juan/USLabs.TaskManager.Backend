using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using USLabs.TaskManager.Data.Entities;

namespace USLabs.TaskManager.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        // Methods for get users
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<IEnumerable<User?>> GetAllUsersAsync();

        // Methods for cruds for user
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(Guid id);
        // Methods for check if userdata exists
        Task<bool> ExistsUserAsync(Guid id);
        Task<bool> ExistsEmailAsync(string email);
    }
}