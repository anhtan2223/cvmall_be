using Application.Core.Contracts;
using Framework.Core.Collections;

namespace Application.Core.Interfaces.Core
{
    public interface IResourceServices
    {
        Task<PagedList<ResourceResponse>> GetPaged(ResourcePagedRequest request);
        List<ResourceResponse> GetByScreen(string lang, string module, string screen);
        Task<int> Create(ResourceRequest request);
        Task<ResourceResponse> GetById(Guid id);
        Task<int> Update(Guid id, ResourceRequest request);
        Task<int> Delete(Guid id);
        Task<PagedList<string>?> GetListScreen(PagedRequest request);
        Task<PagedList<string>?> GetScreensByModule(ResourcePagedRequest request);
    }
}
