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
            var month = request.month != 0 ? request.month : DateTime.Now.Month;
            var year = request.year != 0 ? request.year : DateTime.Now.Year;
            var timesheetQuery = timesheetRepository
                .GetQuery()
                .ExcludeSoftDeleted()
                .Where(x => x.month_year.Month == month && x.month_year.Year == year);

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
                        project_participation_hours = x.timesheet != null ? x.timesheet.project_participation_hours : 0,
                        consumed_hours = x.timesheet != null ? x.timesheet.consumed_hours : 0,
                        late_early_departures = x.timesheet != null ? x.timesheet.late_early_departures : 0,
                        absence_hours = x.timesheet != null ? x.timesheet.absence_hours : 0,
                        Employee = x.employee,
                        Timesheet = x.timesheet,
                })
                .Where(x => x.Employee.EmployeeDepartments == null ? false : x.Employee.EmployeeDepartments.Any(ed => ed.Department.name.Equals("Phòng dự án")))
                .Where(x => string.IsNullOrEmpty(request.search) || x.full_name.ToLower().Contains(request.search.ToLower()))
                .SortBy(request.sort ?? "employee_id.asc");

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

            var timesheetsQuery = query.Select( x => new Timesheet {
                        id = x.Timesheet != null ? x.Timesheet.id : Guid.Empty,
                        employee_id = x.Employee.id,
                        group = x.Timesheet != null ? x.Timesheet.group : null,
                        month_year = x.Timesheet != null ? x.Timesheet.month_year : new DateTime(year, month, 1),
                        project_participation_hours = x.Timesheet != null ? x.Timesheet.project_participation_hours : null,
                        consumed_hours = x.Timesheet != null ? x.Timesheet.consumed_hours : null,
                        late_early_departures = x.Timesheet != null ? x.Timesheet.late_early_departures : null,
                        absence_hours = x.Timesheet != null ? x.Timesheet.absence_hours : null,
                        Employee = x.Employee,
                });

            var data = timesheetsQuery.ToPagedList(request.page, request.size);

            var dataMapping = _mapper.Map<PagedList<TimesheetResponse>>(data);

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

        public async Task<int> UpdateMulti(List<TimesheetRequest> requests)
        {
            var count = 0;

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    foreach (var request in requests)
                    {
                        if(request.id == Guid.Empty)
                        {
                            if (
                                request.group == null &&
                                request.project_participation_hours == 0 &&
                                request.consumed_hours == 0 &&
                                request.late_early_departures == 0 &&
                                request.absence_hours == 0 
                                )
                            {
                                continue;
                            }
                            if (DateTime.Now.Month != request.month_year.Month
                                || DateTime.Now.Year != request.month_year.Year)
                            {
                                throw new ArgumentException();
                            }

                            var entity = _mapper.Map<Timesheet>(request);

                            await timesheetRepository.AddEntityAsync(entity);

                        } else {
                            var entity = timesheetRepository
                            .GetQuery()
                            .FindActiveById(request.id)
                            .FirstOrDefault();
                            if (entity == null)
                                throw new ArgumentException();

                            if(!entity.group.Equals(request.group)) {
                                if (DateTime.Now.Month != request.month_year.Month
                                    || DateTime.Now.Year != request.month_year.Year)
                                {
                                    throw new ArgumentException();
                                }
                                var employee = await employeeRepository
                                .GetQuery()
                                .FindActiveById(entity.employee_id)
                                .FirstOrDefaultAsync();
                                if (employee != null)
                                {
                                    employee.current_group = request.group;
                                    await employeeRepository.UpdateEntityAsync(employee);
                                }
                            }

                            _mapper.Map(request, entity);

                            await timesheetRepository.UpdateEntityAsync(entity);
                        }

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
