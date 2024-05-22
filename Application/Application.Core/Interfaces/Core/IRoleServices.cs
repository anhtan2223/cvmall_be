using Framework.Core.Abstractions;
using Framework.Core.Collections;
using Application.Core.Contracts;

namespace Application.Core.Interfaces.Core
{
    public interface IRoleServices
    {
        Task<PagedList<RoleResponse>> GetPaged(PagedRequest request);
        Task<int> Create(RoleRequest request);
        Task<RoleResponse> GetById(Guid id);
        Task<int> Update(Guid id, RoleRequest request);
        Task<int> Delete(Guid id);
        Task<int> Delete(Guid[] ids);
    }
}
