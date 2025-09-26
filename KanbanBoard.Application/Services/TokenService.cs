using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using KanbanBoard.Application.Configuration;
using KanbanBoard.Application.IServices;
using KanbanBoard.Domain.Entities;
using KanbanBoard.Domain.IRepositories;
using KanbanBoard.Domain.IPersistence;
using Microsoft.IdentityModel.Tokens;

namespace KanbanBoard.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public TokenService(
            JwtOptions jwtOptions,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork)
        {
            _jwtOptions = jwtOptions;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            
            // Initialize token validation parameters
            var key = Encoding.UTF8.GetBytes(_jwtOptions.Key);
            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero // Remove default 5 minute tolerance
            };
        }

        public string CreateAccessToken(User user, IEnumerable<string> roles)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName ?? string.Empty),
                new(ClaimTypes.Email, user.Email ?? string.Empty),
                new("name", user.Name)
            };

            // Add role claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpireMinutes),
                signingCredentials: creds,
                notBefore: DateTime.UtcNow // Token not valid before current time
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken CreateRefreshToken(Guid userId, string clientIpAddress, string userAgent)
        {
            var refreshTokenBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(refreshTokenBytes);
            
            var refreshToken = Convert.ToBase64String(refreshTokenBytes);
            var expiryDate = DateTime.UtcNow.AddDays(7); // 7 days expiry

            return new RefreshToken(refreshToken, expiryDate, userId, clientIpAddress, userAgent);
        }

        public Task<ClaimsPrincipal?> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                
                // Additional validation for JWT
                if (validatedToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return Task.FromResult<ClaimsPrincipal?>(null);
                }

                return Task.FromResult<ClaimsPrincipal?>(principal);
            }
            catch
            {
                return Task.FromResult<ClaimsPrincipal?>(null);
            }
        }

        public async Task<bool> ValidateRefreshTokenAsync(string token)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(token);
            return refreshToken?.IsActive == true;
        }

        public async Task RevokeRefreshTokenAsync(string token, string reason)
        {
            var refreshToken = await _refreshTokenRepository.GetByTokenAsync(token);
            if (refreshToken != null)
            {
                refreshToken.Revoke(reason);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task RevokeAllUserTokensAsync(string userId, string reason)
        {
            await _refreshTokenRepository.RevokeAllUserTokensAsync(userId, reason);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CleanupExpiredTokensAsync()
        {
            await _refreshTokenRepository.CleanupExpiredTokensAsync();
            await _unitOfWork.SaveChangesAsync();
        }

        public string? GetUserIdFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jsonToken = tokenHandler.ReadJwtToken(token);
                return jsonToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
