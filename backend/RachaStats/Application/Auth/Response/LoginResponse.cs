namespace RachaStats.Application.Auth.Response;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresTime { get; set; }
} 