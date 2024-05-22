using Application.Core.Contracts;
using Framework.Core.Collections;

namespace Application.Core.Interfaces
{
    public interface IBaseServices<Request, Search, Response>
    {
        Task<PagedList<Response>> GetPaged(Search request);
        Task<Response> GetById(Guid id);
        Task<int> Create(Request request);
        Task<int> Update(Guid id, Request request);
        Task<int> Delete(Guid id);
        Task<byte[]?> ExportExcel(Request request);
    }
}
