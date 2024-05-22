using Framework.Core.Collections;
using Application.Core.Contracts;
using Application.Core.Services.Core;
using Domain.Entities;

namespace Application.Core.Interfaces.Core
{
    public interface ICvInfoServices
    {
        Task<PagedList<CvInfoResponse>> GetPaged(RequestPaged request);

        Task<IList<CvInfoResponse>> GetList();
        Task<CvInfoResponse> GetById(Guid id);
        Task<int> Create(CvInfoRequest request);
        Task<int> Update(Guid id, CvInfoRequest request);
        Task<int> Delete(Guid id);
    }
}
