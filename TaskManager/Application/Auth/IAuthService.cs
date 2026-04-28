using TaskManager.Application.Auth.Requests;
using TaskManager.Application.Auth.Response;

namespace TaskManager.Application.Auth;

public interface IAuthService
{
    LoginResponse Login(LoginRequest request);
    LoginResponse RefreshToken(RefreshTokensRequest request);
}