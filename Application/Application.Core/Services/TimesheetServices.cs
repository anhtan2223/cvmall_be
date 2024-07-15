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
            var query = GetListQuery(request);

            var data = query.ToPagedList(request.page, request.size);

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
                            var employee = await employeeRepository
                            .GetQuery()
                            .FindActiveById(request.employee_id)
                            .FirstOrDefaultAsync();
                            if (employee != null)
                            {
                                employee.current_group = request.group;
                                await employeeRepository.UpdateEntityAsync(employee);
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

        public async Task<byte[]> ExportAllExcel(TimesheetRequestPaged request)
        {
            var query = GetListQuery(request);

            var dataQuery = query.Select( x => new {
                        id = x != null ? x.id : Guid.Empty,
                        employee_id = x.Employee.id,
                        branch = x.Employee.branch,
                        full_name = x.Employee.full_name,
                        initial_name = x.Employee.initial_name,
                        state = x.Employee.state,
                        employee_code = x.Employee.employee_code,
                        timesheet_id = (Guid?)(x != null ? x.id : null),
                        group = x != null ? x.group : null,
                        month_year = (DateTime?)(x != null ? x.month_year : null),
                        project_participation_hours = x != null ? x.project_participation_hours : 0,
                        consumed_hours = x != null ? x.consumed_hours : 0,
                        late_early_departures = x != null ? x.late_early_departures : 0,
                        absence_hours = x != null ? x.absence_hours : 0,
                });

            var list = dataQuery.ToList();

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
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "EmployeeCode"), key = "employee_code", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "FullName"), key = "full_name", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "Initialname"), key = "initial_name", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "Branch"), key = "branch", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "Group"), key = "group", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "State"), key = "state", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15, 
                        transformFunc = (object state) => {
                            int intState = (int)state;
                            if(intState  < 0 || intState > 3)
                                return "";
                            var map = new[]{
                                "Đang làm việc",
                                "Đang thử việc",
                                "Đang thực tập",
                                "Đã nghỉ việc ",
                            };
                            return map[intState];
                        }},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "ProjectParticipationHours"), key = "project_participation_hours", type = DataType.NUMBER, header_align = CellAlign.CENTER, content_align = CellAlign.CENTER ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "ConsumedHours"), key = "consumed_hours", type = DataType.NUMBER, header_align = CellAlign.CENTER, content_align = CellAlign.CENTER ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "LateEarlyDepartures"), key = "late_early_departures", type = DataType.NUMBER, header_align = CellAlign.CENTER, content_align = CellAlign.CENTER ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "AbsenceHours"), key = "absence_hours", type = DataType.NUMBER, header_align = CellAlign.CENTER, content_align = CellAlign.CENTER ,width=15},
                    };

                    workbookpart?.FillGridData(list, excelItems);

                }
                return ms.ToArray();
            }
        }

        #region private

        private IQueryable<Timesheet> GetListQuery(TimesheetRequestPaged request)
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
                    Employee = employee,
                    Timesheet = timesheetQuery
                        .Where(t => t.employee_id == employee.id)
                        .Take(1)
                        .FirstOrDefault()
                })
                .Select( x => new {
                        id = x.Timesheet != null ? x.Timesheet.id : Guid.Empty,
                        employee_id = x.Employee.id,
                        branch = x.Employee.branch,
                        full_name = x.Employee.full_name,
                        initial_name = x.Employee.initial_name,
                        state = x.Employee.state,
                        employee_code = x.Employee.employee_code,
                        timesheet_id = (Guid?)(x.Timesheet != null ? x.Timesheet.id : null),
                        group = x.Timesheet != null ? x.Timesheet.group : null,
                        month_year = (DateTime?)(x.Timesheet != null ? x.Timesheet.month_year : null),
                        project_participation_hours = x.Timesheet != null ? x.Timesheet.project_participation_hours : 0,
                        consumed_hours = x.Timesheet != null ? x.Timesheet.consumed_hours : 0,
                        late_early_departures = x.Timesheet != null ? x.Timesheet.late_early_departures : 0,
                        absence_hours = x.Timesheet != null ? x.Timesheet.absence_hours : 0,
                        Employee = x.Employee,
                        Timesheet = x.Timesheet,
                })
                .Where(x => x.Employee.EmployeeDepartments == null ? false : x.Employee.EmployeeDepartments.Any(ed => ed.Department.name.Equals("Phòng Sản xuất")))
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

            return timesheetsQuery;
        }
        
        #endregion
    }
}
