using Microsoft.EntityFrameworkCore;
using USLabs.TaskManager.Data.Context;
using USLabs.TaskManager.Data.Entities;
using USLabs.TaskManager.Data.Repositories.Interfaces;

namespace USLabs.TaskManager.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TaskManagerContext _context;

        public UserRepository(TaskManagerContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users
                .Include(u => u.Categories)
                .Include(u => u.TaskItems)
                .FirstOrDefaultAsync(u => u.Id == id);
        }        

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email!.ToLower() == email.ToLower());
        }

        public async Task<IEnumerable<User?>> GetAllUsersAsync()
        {
            return await _context.Users
                .Include(u => u.Categories)
                .Include(u => u.TaskItems)
                .ToListAsync();
        }

        public async Task<User> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        
        public Task<bool> ExistsUserAsync(Guid id)
        {
            return _context.Users.AnyAsync(u => u.Id == id);
        }

        public Task<bool> ExistsEmailAsync(string email)
        {
            return _context.Users.AnyAsync(u => u.Email!.ToLower() == email.ToLower());
        }
    }
}