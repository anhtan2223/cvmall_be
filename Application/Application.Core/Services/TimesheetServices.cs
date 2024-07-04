using AutoMapper;
using Framework.Core.Collections;
using Framework.Core.Extensions;
using Application.Core.Contracts;
using Domain.Entities;
using Application.Common.Abstractions;
using Application.Core.Interfaces.Core;
using Microsoft.EntityFrameworkCore;
using DocumentFormat.OpenXml.Packaging;
using Framework.Core.Helpers;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO.Compression;
using Microsoft.IdentityModel.Tokens;
using Framework.Core.Helpers.Excel;
using Application.Common.Extensions;

namespace Application.Core.Services.Core
{
    public class TimesheetServices : BaseService, ITimesheetServices
    {
        private readonly IRepository<Timesheet> timesheetRepository;
        private readonly IRepository<Employee> employeeRepository;
        private ILocalizeServices ls { get; set; }

        public TimesheetServices(IUnitOfWork _unitOfWork, IMapper _mapper, ILocalizeServices _localServices) : base(_unitOfWork, _mapper)
        {
            timesheetRepository = _unitOfWork.GetRepository<Timesheet>();
            employeeRepository = _unitOfWork.GetRepository<Employee>();
            ls = _localServices;
        }

        public async Task<PagedList<TimesheetResponse>> GetPaged(TimesheetRequestPaged request)
        {

            var timesheetQuery = timesheetRepository
                .GetQuery()
                .ExcludeSoftDeleted()
                .Where(x => x.month_year.Month == request.month && x.month_year.Year == request.year);

            var query = employeeRepository
                .GetQuery()
                .ExcludeSoftDeleted()
                .Select(employee => new
                {
                    employee = employee,
                    timesheet = timesheetQuery
                        .Where(t => t.employee_id == employee.id)
                        .Take(1)
                        .FirstOrDefault()
                })
                .Select( x => new {
                        employee_id = x.employee.id,
                        branch = x.employee.branch,
                        full_name = x.employee.full_name,
                        initial_name = x.employee.initial_name,
                        state = x.employee.state,
                        employee_code = x.employee.employee_code,
                        timesheet_id = (Guid?)(x.timesheet != null ? x.timesheet.id : null),
                        group = x.timesheet != null ? x.timesheet.group : null,
                        month_year = (DateTime?)(x.timesheet != null ? x.timesheet.month_year : null),
                        project_participation_hours = x.timesheet != null ? x.timesheet.project_participation_hours : null,
                        consumed_hours = x.timesheet != null ? x.timesheet.consumed_hours : null,
                        late_early_departures = x.timesheet != null ? x.timesheet.late_early_departures : null,
                        absence_hours = x.timesheet != null ? x.timesheet.absence_hours : null,
                        Employee = x.employee,
                        Timesheet = x.timesheet,
                })
                .Where(x => string.IsNullOrEmpty(request.search) || x.full_name.ToLower().Contains(request.search.ToLower()))
                .SortBy(request.sort);


            if (!request.branchFilters.IsNullOrEmpty())
            {
                query = query.Where(x => request.branchFilters.Contains(x.branch));
            }

            if (!request.groupFilters.IsNullOrEmpty())
            {
                query = query.Where(x => request.groupFilters.Contains(x.group));
            }

            if (!request.stateFilters.IsNullOrEmpty())
            {
                query = query.Where(x => request.stateFilters.Contains(x.state));
            }

            var data = query.ToPagedList(request.page, request.size);

            var timesheets = new List<Timesheet>();

            foreach (var item in data.data)
            {
                if (item.Timesheet != null)
                {
                    timesheets.Add(item.Timesheet);
                }
                else
                {
                    timesheets.Add(new Timesheet
                    {
                        employee_id = item.Employee.id,
                        month_year = new DateTime(request.year, request.month, 1),
                        Employee = item.Employee,
                    });
                }
            }

            PagedList<Timesheet> pagedList = timesheets.ToPagedList(request.page, request.size);

            var dataMapping = _mapper.Map<PagedList<TimesheetResponse>>(pagedList);

            return dataMapping;
        }

        public async Task<TimesheetResponse> GetById(Guid id)
        {
            var entity = timesheetRepository
                                  .GetQuery()
                                  .ExcludeSoftDeleted()
                                  .FilterById(id)
                                  .FirstOrDefault();

            var data = _mapper.Map<TimesheetResponse>(entity);

            return data;
        }

        public async Task<int> Create(TimesheetRequest request)
        {
            var count = 0;

            // TODO: ???
            // request = ValidateRequest(request);

            var timesheet = _mapper.Map<Timesheet>(request);

            await timesheetRepository.AddEntityAsync(timesheet);

            count += await _unitOfWork.SaveChangesAsync();

            return count;
        }

        public async Task<int> Update(Guid id, TimesheetRequest request)
        {
            var count = 0;

            var entity = timesheetRepository
                            .GetQuery()
                            .FindActiveById(id)
                            .FirstOrDefault();


            _mapper.Map(request, entity);

            await timesheetRepository.UpdateEntityAsync(entity);

            count = await _unitOfWork.SaveChangesAsync();
            return count;
        }

        public async Task<int> UpdateList(List<TimesheetRequest> requests)
        {
            // TODO: do not create timesheet if that timesheet data is empty
            var count = 0;

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    foreach (var request in requests)
                    {
                        var entity = timesheetRepository
                            .GetQuery()
                            .FindActiveById(request.id)
                            .FirstOrDefault();

                        if (entity == null)
                            continue;

                        _mapper.Map(request, entity);

                        await timesheetRepository.UpdateEntityAsync(entity);

                        count++;
                    }

                    await _unitOfWork.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    count = -1;
                }
            }

            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var entity = timesheetRepository.GetQuery().FindActiveById(id).FirstOrDefault();

            if (entity == null)
                return 0;

            await timesheetRepository.DeleteEntityAsync(entity);
            var count = await _unitOfWork.SaveChangesAsync();
            return count;
        }

        public async Task<byte[]> ExportAllExcelByMonthYear(int month, int year)
        {
            List<Timesheet> list = await GetListByMonthYear(month, year);

            if (list == null)
                return null;

            using (var ms = new MemoryStream())
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
                {
                    document.CreateWorkbookPart();
                    WorkbookPart? workbookpart = document.WorkbookPart;
                    workbookpart?.CreateSheet("Sheet1");

                    List<ExcelItem> excelItems = new() {
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "TimesheetName"), key = "timesheet_name", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "FullName"), key = "full_name", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=30},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "Email"), key = "mail", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "Phone"), key = "phone", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                    };

                    workbookpart?.FillGridData(list, excelItems);

                }
                return ms.ToArray();
            }
        }

        #region private


        private async Task<List<Timesheet>> GetListByMonthYear(int month, int year)
        {
            var data = timesheetRepository
                .GetQuery()
                .ExcludeSoftDeleted()
                .Where(x => x.month_year.Month == month && x.month_year.Year == year)
                .SortBy("employee_id.desc")
                .Take(9999)
                .ToList();

            var dataMapping = new List<TimesheetResponse>();

            return data;
        }
        #endregion
    }
}
