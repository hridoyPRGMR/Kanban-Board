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

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BoardDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BoardDto?> GetByidAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Guid id, CreateUpdateBoardDto dto)
        {
            throw new NotImplementedException();
        }
    }

}