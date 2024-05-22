using Framework.Core.Collections;
using Application.Core.Contracts;
using Domain.Entities;

namespace Application.Core.Interfaces.Core
{
    public interface ITechnicalServices
    {
        Task<IList<TechnicalResponse>> GetList();
        Task<PagedList<TechnicalResponse>> GetPaged(RequestPaged request);
        Task<TechnicalResponse> GetById(Guid id);
        Task<int> Create(TechnicalRequest request);
        Task<int> Update(Guid id, TechnicalRequest request);
        Task<int> Delete(Guid id);
    }
}
