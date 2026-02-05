using System;
using KanbanBoard.Application.IServices;
using KanbanBoard.Shared.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BoardController : ControllerBase
    {

        private readonly IBoardService _boardService;

        public BoardController(
            IBoardService boardService
        )
        {
            _boardService = boardService;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateUpdateBoardDto input)
        {
            await _boardService.CreateAsync(input);
            return Ok();
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var board = await _boardService.GetByidAsync(id);
            return board is null ? NotFound() : Ok(board);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id,CreateUpdateBoardDto input)
        {
            var result = await _boardService.UpdateAsync(id, input);
            if (!result.IsSuccess)
            {
                var msg = result.ErrorMessage ?? ""
;
                if (msg.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound(new { message = msg });

                return BadRequest(new { message = msg });
            }

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            return NoContent();
        }
    }
}