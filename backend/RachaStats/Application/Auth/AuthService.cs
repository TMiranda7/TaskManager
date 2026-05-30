using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RachaStats.Application.Auth.Requests;
using RachaStats.Application.Auth.Response;
using RachaStats.Application.Common.Exceptions;
using RachaStats.Domain.Entities;
using RachaStats.Infrastructure.Data;

namespace RachaStats.Application.Auth;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    
    public AuthService(IConfiguration configuration, AppDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }
    
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var username = request.Username.Trim();
        var passwordHash = PasswordHasher.ComputeSha256(request.Password);
        var user = await _context.AppUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(appUser => appUser.Username == username && appUser.PasswordHash == passwordHash);

        if (user is null)
            throw new BusinessException("Usuario ou senha invalidos");
        
        var accessToken = GenerateJwt(user.Username, user.Role);
        var refreshToken = Guid.NewGuid().ToString();

        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = refreshToken,
            Username = user.Username,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        };
        
        await _context.RefreshTokens.AddAsync(refreshTokenEntity);
        await _context.SaveChangesAsync();

        return new LoginResponse
        {
            Token = accessToken,
            ExpiresTime = GetAccessTokenExpiration(),
            RefreshToken = refreshToken
        };
    }

    public async Task<LoginResponse> RefreshTokenAsync(RefreshTokensRequest request)
    {
        var storedToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(x => x.Token == request.RefreshToken);

        if (storedToken == null)
            throw new BusinessException("Refresh token inválido");

        if (storedToken.IsRevoked)
            throw new BusinessException("Refresh token já foi revogado");

        if (storedToken.ExpiresAt < DateTime.UtcNow)
            throw new BusinessException("Refresh token expirado");

        storedToken.IsRevoked = true;
        
        var newRefreshToken = Guid.NewGuid().ToString();

        var newRefreshTokenItem = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = newRefreshToken,
            Username = storedToken.Username,
            ExpiresAt = DateTime.UtcNow.AddDays(2),
            IsRevoked = false
        };
        
        await _context.RefreshTokens.AddAsync(newRefreshTokenItem);
        
        var userRole = await _context.AppUsers
            .AsNoTracking()
            .Where(appUser => appUser.Username == storedToken.Username)
            .Select(appUser => appUser.Role)
            .FirstOrDefaultAsync();

        if (string.IsNullOrWhiteSpace(userRole))
            throw new BusinessException("Usuario do refresh token nao foi encontrado");

        var newAccessToken = GenerateJwt(storedToken.Username, userRole);
        
        await _context.SaveChangesAsync();

        return new LoginResponse
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresTime = GetAccessTokenExpiration()
        };
    }
    
    private string GenerateJwt(string username, string role)
    {
        var key = _configuration["Jwt:Key"]!;
        var issuer = _configuration["Jwt:Issuer"]!;
        var audience = _configuration["Jwt:Audience"]!;
        var expiresInMinutes = int.Parse(_configuration["Jwt:ExpiresInMinutes"]!);

        var expiresAt = DateTime.UtcNow.AddMinutes(expiresInMinutes);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private DateTime GetAccessTokenExpiration()
    {
        var expiresInMinutes = int.Parse(_configuration["Jwt:ExpiresInMinutes"]!);
        return DateTime.UtcNow.AddMinutes(expiresInMinutes);
    }
}
