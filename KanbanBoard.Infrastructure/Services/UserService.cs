using KanbanBoard.Application.Dtos;
using KanbanBoard.Application.IServices;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.IPersistence;
using KanbanBoard.Domain.IRepositories;

namespace KanbanBoard.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task InsertUser(RegisterUserDto input)
        {

            var user = new User(
                input.Email,
                input.PasswordHash,
                input.UserName,
                input.Name,
                input.PhoneNumber);

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

    }

}