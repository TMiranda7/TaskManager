using RachaStats.Application.Auth.Requests;
using RachaStats.Application.Auth.Response;

namespace RachaStats.Application.Auth;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<LoginResponse> RefreshTokenAsync(RefreshTokensRequest request);
}
