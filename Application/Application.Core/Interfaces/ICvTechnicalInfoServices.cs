using Application.Core.Contracts;
using Framework.Core.Collections;

namespace Application.Core.Interfaces.Core
{
    public interface ICvTechnicalInfoServices
    {
        Task<PagedList<CvTechInfoResponse>> GetPaged(RequestPaged request);
        Task<IList<CvTechInfoResponse>> GetList();
        Task<CvTechInfoResponse> GetById(Guid id);
        Task<int> Create(CvTechnicalInfoRequest request);
        Task<int> Update(Guid id, CvTechnicalInfoRequest request);
        Task<int> Delete(Guid id);
    }
}
