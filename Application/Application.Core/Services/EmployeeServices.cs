using AutoMapper;
using Framework.Core.Collections;
using Framework.Core.Extensions;
using Application.Core.Contracts;
using Domain.Entities;
using Application.Common.Abstractions;
using Application.Core.Interfaces.Core;
// using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using Framework.Core.Helpers;
using Framework.Core.Helpers.Excel;
using Application.Common.Extensions;


namespace Application.Core.Services.Core
{
    public class EmployeeServices : BaseService, IEmployeeServices
    {
        private readonly IRepository<Employee> employeeRepository;
        private ILocalizeServices ls { get; set; }


        public EmployeeServices(IUnitOfWork _unitOfWork, IMapper _mapper, ILocalizeServices _ls) : base(_unitOfWork, _mapper)
        {
            employeeRepository = _unitOfWork.GetRepository<Employee>();
            ls = _ls;
        }

        public async Task<PagedList<EmployeeResponse>> GetPaged(RequestEmployeePaged request)
        {

            var data = await employeeRepository
                    .GetQuery()
                    .ExcludeSoftDeleted()
                    .Where(x => (request.state == null) || request.state.Contains(x.state))
                    .Where(x => (request.group == null) || request.group.Contains(x.current_group))
                    .Where(x => (request.branch == null) || request.branch.Contains(x.branch))
                    .Where(x => (request.department == null) || x.EmployeeDepartments.Any(y => request.department.Contains(y.Department.name)))
                    .Where(x => (request.position == null) || x.EmployeePositions.Any(y => request.position.Contains(y.Position.name)))
                    .Where(x => string.IsNullOrEmpty(request.search) || (x.full_name.ToLower().Contains(request.search.ToLower()) || x.initial_name.ToLower().Contains(request.search.ToLower())))
                    .SortBy(request.sort ?? "updated_at.desc")
                    .Include(x => x.EmployeePositions)
                        .ThenInclude(ep => ep.Position)
                    .Include(x => x.EmployeeDepartments)
                        .ThenInclude(ep => ep.Department)
                    .Include(x => x.Timesheets)
                    .ToPagedListAsync(request.page, request.size);

            foreach (var employee in data.data)
            {
                employee.EmployeePositions = employee?.EmployeePositions?.Where(x => !x.del_flg).ToList();
                employee.EmployeeDepartments = employee?.EmployeeDepartments?.Where(x => !x.del_flg).ToList();
            }

            var dataMapping = _mapper.Map<PagedList<EmployeeResponse>>(data);

            return dataMapping;
        }

        public async Task<IList<EmployeeResponse>> GetList()
        {
            var data = await employeeRepository
                .GetQuery()
                .ExcludeSoftDeleted()
                .SortBy("updated_at.desc").ToPagedListAsync(1, 9999);

            List<EmployeeResponse> dataMapping = new();

            if (data?.data?.Count > 0)
            {
                dataMapping = _mapper.Map<IList<Employee>, List<EmployeeResponse>>(data.data);
            }

            return dataMapping;
        }

        public async Task<EmployeeResponse> GetById(Guid id)
        {
            var entity = employeeRepository
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .Include(x => x.EmployeePositions)
                                        .ThenInclude(ep => ep.Position)
                                    .Include(x => x.EmployeeDepartments)
                                        .ThenInclude(ep => ep.Department)
                                    .Include(x => x.Timesheets)
                                    .FilterById(id)
                                    .FirstOrDefault();

            if (entity != null)
            {
                entity.EmployeePositions = entity?.EmployeePositions
                                                ?.Where(x => !x.del_flg)
                                                .ToList();
                entity.EmployeeDepartments = entity?.EmployeeDepartments
                                                ?.Where(x => !x.del_flg)
                                                .ToList();
            }

            var data = _mapper.Map<EmployeeResponse>(entity);

            return data;
        }

        public async Task<bool> CheckEmployeeCode(string employeeCode)
        {
            return await _unitOfWork.GetRepository<Employee>()
                                    .GetQuery()
                                    .AnyAsync(x => x.employee_code == employeeCode);
        }

        public async Task<int> Create(EmployeeRequest request)
        {
            var count = 0;

            string pattern = @"^VHEC-\d+$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(request.employee_code) || await CheckEmployeeCode(request.employee_code))
            {
                return 0;
            }

            var Employee = _mapper.Map<Employee>(request);

            await employeeRepository.AddEntityAsync(Employee);

            count += await _unitOfWork.SaveChangesAsync();

            return count;
        }

        public async Task<int> Update(Guid id, EmployeeRequest request)
        {
            var count = 0;

            var entity = _unitOfWork
                            .GetRepository<Employee>()
                            .GetQuery()
                            .FindActiveById(id)
                            .FirstOrDefault();

            if (entity == null)
                return count;

            string pattern = @"^VHEC-\d+$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(request.employee_code) || (await CheckEmployeeCode(request.employee_code) && entity.employee_code != request.employee_code))
            {
                return count;
            }



            _mapper.Map(request, entity);
            await employeeRepository.UpdateEntityAsync(entity);

            var employeePositionEntityList = _unitOfWork.GetRepository<EmployeePosition>()
                                            .GetQuery()
                                            .ExcludeSoftDeleted()
                                            .Where(x => x.employee_id == id);

            var employeeDepartmentEntityList = _unitOfWork.GetRepository<EmployeeDepartment>()
                                                .GetQuery()
                                                .ExcludeSoftDeleted()
                                                .Where(x => x.employee_id == id);

            var employeePositionRequestList = request.EmployeePositions;

            var employeeDepartmentRequestList = request.EmployeeDepartments;

            if (employeePositionRequestList?.Count > 0)
            {
                // Delete or update position
                foreach (var itemEmployeePositionEntity in employeePositionEntityList)
                {
                    var EPinRq = employeePositionRequestList.Find(x => x.id == itemEmployeePositionEntity.id);

                    if (EPinRq == null)
                    {
                        await _unitOfWork.GetRepository<EmployeePosition>()
                                    .DeleteEntityAsync(itemEmployeePositionEntity);
                    }
                }
            }
            else
            {
                foreach (var itemEmployeePositionEntity in employeePositionEntityList)
                {
                    await _unitOfWork.GetRepository<EmployeePosition>()
                                    .DeleteEntityAsync(itemEmployeePositionEntity);
                }
            }

            if (employeeDepartmentRequestList?.Count > 0)
            {
                // Update or Delete Department
                foreach (var itemEmployeeDepartmentEntity in employeeDepartmentEntityList)
                {
                    var EDinRq = employeeDepartmentRequestList.Find(x => x.id == itemEmployeeDepartmentEntity.id);

                    if (EDinRq == null)
                    {
                        await _unitOfWork.GetRepository<EmployeeDepartment>()
                                    .DeleteEntityAsync(itemEmployeeDepartmentEntity);
                    }
                }
            }
            else
            {
                foreach (var itemEmployeeDepartmentEntity in employeeDepartmentEntityList)
                {
                    await _unitOfWork.GetRepository<EmployeeDepartment>()
                                    .DeleteEntityAsync(itemEmployeeDepartmentEntity);
                }
            }

            count = await _unitOfWork.SaveChangesAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var entity = employeeRepository.GetQuery().FindActiveById(id).FirstOrDefault();

            if (entity == null)
                return 0;

            await employeeRepository.DeleteEntityAsync(entity);
            var count = await _unitOfWork.SaveChangesAsync();
            return count;
        }

        public async Task<IList<EmployeeResponse>> GetGroups()
        {
            var data = await employeeRepository
                .GetQuery()
                .ExcludeSoftDeleted()
                .Where(x => x.EmployeePositions.Any(
                    y => y.Position.name.ToLower() == "team leader" ||
                    y.Position.name.ToLower() == "trưởng nhóm")
                )
                .ToPagedListAsync(1, 9999);

            List<EmployeeResponse> dataMapping = new();

            if (data?.data?.Count > 0)
            {
                dataMapping = _mapper.Map<IList<Employee>, List<EmployeeResponse>>(data.data);
            }

            return dataMapping;
        }


        public async Task<byte[]> ExportAllExcel()
        {

            var list = await GetAll();

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

        public async Task<byte[]> ExportTemplateExcel()
        {

            var list = await GetAll();

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
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "employee_code"), key = "employee_code", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "branch"), key = "branch", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "full_name"), key = "full_name", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "initial_name"), key = "initial_name", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "current_group"), key = "current_group", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "state"), key = "state", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "company_email"), key = "company_email", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "personal_email"), key = "personal_email", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "phone"), key = "phone", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "birthday"), key = "birthday", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "permanent_address"), key = "permanent_address", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "current_address"), key = "current_address", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "id_number"), key = "id_number", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "date_issue"), key = "date_issue", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "location_issue"), key = "location_issue", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                        new ExcelItem(){ header =  ls.Get(Modules.Core, Screen.Timesheet, "is_married"), key = "is_married", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT ,width=15},
                    };

                    workbookpart?.FillGridData(list, excelItems);

                }
                return ms.ToArray();
            }
        }


        #region private


        private async Task<List<Employee>> GetAll()
        {
            var data = employeeRepository
                .GetQuery()
                .ExcludeSoftDeleted()
                .Take(9999)
                .ToList();

            return data;
        }

        #endregion
    }
}
