using KanbanBoard.Application.Dtos;
using KanbanBoard.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task Register(RegisterUserDto input)
        {
            await _userService.InsertUser(input);
        }
    }
}