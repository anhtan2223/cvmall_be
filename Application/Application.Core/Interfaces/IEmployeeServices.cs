using Framework.Core.Collections;
using Application.Core.Contracts;
using Domain.Entities;

namespace Application.Core.Interfaces.Core
{
    public interface IEmployeeServices
    {
        Task<PagedList<EmployeeResponse>> GetPaged(RequestEmployeePaged request);

        Task<IList<EmployeeResponse>> GetList();
        Task<EmployeeResponse> GetById(Guid id);
        Task<bool> CheckEmployeeCode(string employeeCode);
        Task<int> Create(EmployeeRequest request);
        Task<int> Update(Guid id, EmployeeRequest request);
        Task<int> Delete(Guid id);
        Task<IList<EmployeeResponse>> GetGroups();
    }
}
