using Framework.Core.Abstractions;
using Application.Core.Contracts;
using Framework.Core.Collections;

namespace Application.Core.Interfaces.Core
{
    public interface IFunctionServices
    {
        Task<PagedList<FunctionResponse>> GetPaged(FunctionPagedRequest request);
        Task<int> Create(FunctionRequest request);
        Task<FunctionResponse> GetById(Guid id);
        Task<int> Update(Guid id, FunctionRequest request);
        Task<int> Delete(Guid id);
        Task<int> Delete(Guid[] ids);
        Task<int> ChangeIsActive(Guid[] ids);
        Task<List<FunctionResponse>> GetAsTreeView();
    }
}
