using System;
using System.Security.Claims;
using KanbanBoard.Application.IServices;
using Microsoft.AspNetCore.Http;

namespace KanbanBoard.API.Services
{
    public class HttpContextCurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextCurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user == null || !user.Identity?.IsAuthenticated == true)
                    return null;

                // Prefer NameIdentifier claim (used in this project) and fall back to "sub"
                var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? user.FindFirst("sub")?.Value;
                if (string.IsNullOrWhiteSpace(idClaim)) return null;

                if (Guid.TryParse(idClaim, out var guid))
                    return guid;

                return null;
            }
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public Guid GetRequiredUserId() => UserId ?? throw new InvalidOperationException("User must be authenticated");

    }
    
}
