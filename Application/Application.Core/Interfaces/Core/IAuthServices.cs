using Application.Core.Contracts;

namespace Application.Core.Interfaces.Core
{
    public interface IAuthServices
    {
        Task<AuthLoginResponse> Login(AuthLoginRequest request);
        Task<AuthLoginResponse> Refresh(string refresh_token);
        Task<int> ChangePassword(Guid id, AuthChangePassRequest request);
        bool CheckUserAuthorized(Guid id, string path, string action);
    }
}
