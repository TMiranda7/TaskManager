using RachaStats.Application.Auth.Requests;
using RachaStats.Application.Auth.Response;

namespace RachaStats.Application.Auth;

public interface IAuthService
{
    LoginResponse Login(LoginRequest request);
    LoginResponse RefreshToken(RefreshTokensRequest request);
}