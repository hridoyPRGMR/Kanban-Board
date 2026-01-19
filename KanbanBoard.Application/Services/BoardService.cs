using AutoMapper;
using KanbanBoard.Application.IServices;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.IPersistence;
using KanbanBoard.Domain.IRepositories;
using KanbanBoard.Shared.Dtos;
using KanbanBoard.Shared.Exceptions;

namespace KanbanBoard.Application.Services
{
    public class BoardService : IBoardService
    {

        private readonly IBoardRepository _boardRepository;
        private readonly IProjectRepository _projectRepository;
         private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BoardService(
            IBoardRepository boardRepository,
            IProjectRepository projectRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {
            _boardRepository = boardRepository;
            _projectRepository = projectRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<BoardDto> CreateAsync(CreateUpdateBoardDto input)
        {

            bool projectExist = await _projectRepository.ExistById(input.ProjectId);
            if(!projectExist)
                throw new NotFoundException("Project not found");

            var board =  Board.Create(input.Name,input.ProjectId,input.Description);
            var savedBoard = await _boardRepository.AddAsync(board);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<BoardDto>(savedBoard);
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var board = await _boardRepository.GetByIdAsync(id);
            if (board is null)
                return Result.Failure("Board not found");

            await _boardRepository.RemoveAsync(board);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public Task<IEnumerable<BoardDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<BoardDto?> GetByidAsync(Guid id)
        {
            var board = await _boardRepository.GetByIdAsync(id);
            return _mapper.Map<BoardDto>(board);
        }

        public async Task<Result> UpdateAsync(Guid id, CreateUpdateBoardDto dto)
        {
            var board = await _boardRepository.GetByIdAsync(id);
            if (board is null)
                return Result.Failure("Board not found");

            board.Update(dto.Name, dto.Description);
            await _boardRepository.UpdateAsync(board);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
    }

}