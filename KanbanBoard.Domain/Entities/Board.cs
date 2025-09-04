namespace KanbanBoard.Domain.Entities
{
    public class Board
    {
        public string Name { get; private set; }
        public string ProjectId { get; private set; }
        public Project Project { get; private set; }
        protected Board() { }
    }
}