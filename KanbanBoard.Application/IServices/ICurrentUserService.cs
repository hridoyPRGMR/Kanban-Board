using System;

namespace KanbanBoard.Application.IServices
{
    /// <summary>
    /// Provides information about the currently authenticated user.
    /// Implementations should live in the presentation/API layer and be registered in DI.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Gets the current user's id as a Guid if available, otherwise null.
        /// </summary>
        Guid? UserId { get; }

        /// <summary>
        /// Whether a user is authenticated.
        /// </summary>
        bool IsAuthenticated { get; }
    }
}
