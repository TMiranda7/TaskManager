namespace TaskManager.Application.Auth.Requests;

public class RefreshTokensRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}