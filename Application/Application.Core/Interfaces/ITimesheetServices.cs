using Framework.Core.Collections;
using Application.Core.Contracts;
using Application.Core.Services.Core;
using Domain.Entities;

namespace Application.Core.Interfaces.Core
{
    public interface ITimesheetServices
    {
        Task<PagedList<TimesheetResponse>> GetPaged(TimesheetRequestPaged request);
        // Task<TimesheetResponse> GetById(Guid id);
        // Task<int> Create(TimesheetRequest request);
        // Task<int> Update(Guid id, TimesheetRequest request);
        Task<int> UpdateList(List<TimesheetRequest> requestList);
        // Task<int> Delete(Guid id);
        Task<byte[]> ExportAllExcelByMonthYear( int month, int year);
    }
}
