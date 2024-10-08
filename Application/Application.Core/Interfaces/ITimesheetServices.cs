﻿using Framework.Core.Collections;
using Application.Core.Contracts;
using Application.Core.Services.Core;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Core.Interfaces.Core
{
    public interface ITimesheetServices
    {
        Task<PagedList<TimesheetResponse>> GetPaged(TimesheetRequestPaged request);
        // Task<TimesheetResponse> GetById(Guid id);
        // Task<int> Create(TimesheetRequest request);
        // Task<int> Update(Guid id, TimesheetRequest request);
        Task<int> UpdateMulti(List<TimesheetRequest> requestList);
        // Task<int> Delete(Guid id);
        Task<byte[]> ExportAllExcel(TimesheetRequestPaged request);
        Task<int> ImportLateEarly(IFormFile file , DateTime month) ;
    }
}
