using Application.Core.Contracts;
using Framework.Core.Collections;

namespace Application.Core.Interfaces.Core
{
    public interface ITechnicalCategoryServices
    {
        Task<PagedList<TechnicalCategoryResponse>> GetPaged(RequestPaged request);
        Task<IList<TechnicalCategoryResponse>> GetList();
        Task<TechnicalCategoryResponse> GetById(Guid id);
        Task<int> Create(TechnicalCategoryRequest request);
        Task<int> Update(Guid id, TechnicalCategoryRequest request);
        Task<int> Delete(Guid id);
    }
}
