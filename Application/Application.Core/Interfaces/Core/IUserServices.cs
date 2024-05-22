using Framework.Core.Collections;
using Application.Core.Contracts;

namespace Application.Core.Interfaces.Core
{
    public interface IUserServices
    {
        Task<PagedList<UserResponse>> GetPaged(UserSearchRequest request);
        Task<UserResponse> GetById(Guid id);
        Task<UserResponse> GetInfoLoginById(Guid id);
        Task<int> Create(UserRequest request);
        Task<int> Update(Guid id, UserRequest request);
        Task<int> Delete(Guid id);
        string GetUserNameById(Guid id);
    }
}
