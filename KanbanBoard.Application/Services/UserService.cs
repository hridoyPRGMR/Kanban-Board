using KanbanBoard.Application.IServices;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.IRepositories;

namespace KanbanBoard.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> FindByIdAsync(string userId)
        {
            if (Guid.TryParse(userId, out var guidId))
            {
                return await _userRepository.GetByIdAsync(guidId);
            }
            return null;
        }

        public async Task<User?> FindByUsernameAsync(string username)
        {
            return await _userRepository.GetByUsernameAsync(username);
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }
    }
}