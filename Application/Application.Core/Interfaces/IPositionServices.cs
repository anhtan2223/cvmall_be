using Framework.Core.Collections;
using Application.Core.Contracts;
using Domain.Entities;

namespace Application.Core.Interfaces.Core
{
    public interface IPositionServices
    {
        Task<IList<PositionResponse>> GetList();
        Task<PositionResponse> GetById(Guid id);
        Task<int> Create(PositionRequest request);
        Task<int> Update(Guid id, PositionRequest request);
        Task<int> Delete(Guid id);
    }
}
