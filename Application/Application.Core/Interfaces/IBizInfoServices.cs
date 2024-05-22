using Framework.Core.Collections;
using Application.Core.Contracts;

namespace Application.Core.Interfaces.Core
{
    public interface IBizInfoServices
    {
        Task<PagedList<BizInfoResponse>> GetPaged(RequestPaged request);

        Task<IList<BizInfoResponse>> GetList();
        Task<BizInfoResponse> GetById(Guid id);
        Task<int> Create(BizInfoRequest request);
        Task<int> Update(Guid id, BizInfoRequest request);
        Task<int> Delete(Guid id);
    }
}
