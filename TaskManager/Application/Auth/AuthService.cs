using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Application.Auth.Requests;
using TaskManager.Application.Auth.Response;
using TaskManager.Application.Common.Exceptions;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Data;

namespace TaskManager.Application.Auth;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    
    public AuthService(IConfiguration configuration, AppDbContext context)
    {
            _configuration = configuration;
            _context = context;
    }
    
    public LoginResponse Login(LoginRequest request)
    {
        if (!IsValideToken(request))
            throw new BusinessException("Usuario ou senha invalidos");
        
        var accessToken = GenerateJwt(request.Username);
        var refreshToken = Guid.NewGuid().ToString();

        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = refreshToken,
            Username = request.Username,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IsRevoked = false
        };
        
        _context.RefreshTokens.Add(refreshTokenEntity);
        _context.SaveChanges();

        return new LoginResponse
        {
            Token = accessToken,
            ExpiresTime = GetAccessTokenExpiration(),
            RefreshToken = refreshToken
        };
    }
    public LoginResponse RefreshToken(RefreshTokensRequest request)
    {
        var storedToken = _context.RefreshTokens
            .FirstOrDefault(x => x.Token == request.RefreshToken);

        if (storedToken == null)
            throw new BusinessException("Refresh token inválido");

        if (storedToken.IsRevoked)
            throw new BusinessException("Refresh token já foi revogado");

        if (storedToken.ExpiresAt < DateTime.UtcNow)
            throw new BusinessException("Refresh token expirado");

        storedToken.IsRevoked = true;

        var newAccessToken = GenerateJwt(storedToken.Username);

        _context.SaveChanges();

        return new LoginResponse
        {
            Token = newAccessToken,
            RefreshToken = request.RefreshToken, // você pode trocar depois (rotacionar)
            ExpiresTime = DateTime.UtcNow.AddMinutes(
                int.Parse(_configuration["Jwt:ExpiresInMinutes"]!)
            )
        };
    }
    
    private string GenerateJwt(string username)
    {
        var key = _configuration["Jwt:Key"]!;
        var issuer = _configuration["Jwt:Issuer"]!;
        var audience = _configuration["Jwt:Audience"]!;
        var expiresInMinutes = int.Parse(_configuration["Jwt:ExpiresInMinutes"]!);

        var expiresAt = DateTime.UtcNow.AddMinutes(expiresInMinutes);

        string role = username == "admin" ? "Admin" : "User";

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, GetRole(username))
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
    
    private static bool IsValideToken(LoginRequest request)
    {
        return (request.Username == "admin" && request.Password == "123456") ||
               (request.Username == "user" && request.Password == "123456");

    }

    private static string GetRole(string userName)
    {
        return userName == "admin" ? "Admin" : "User";
    }
}