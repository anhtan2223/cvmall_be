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
        private string _templatePath = "../../Presentation/WebAPI/Assets/Employee/Employee_Template.xlsx";


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
                    //.Include(x => x.Timesheets)
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
                                    //.Include(x => x.Timesheets)
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
                await AddFileToMemoryStream(ms, _templatePath);
                using (SpreadsheetDocument document = SpreadsheetDocument.Open(ms, true))
                {
                    string[] states = { "Đang làm việc", "Đang thử việc", "Đang thực tập", "Đã nghỉ việc" };
                    

                    WorkbookPart? workbookpart = document.WorkbookPart;
                    foreach(var (employee, index) in list.data.Select((item, index) => (item, index)))
                    {   
                        int row = index + 2;

                        List<string> departments = new List<string>();
                        List<string> positions = new List<string>();
                        
                        foreach (var eDepartment in employee.EmployeeDepartments)
                        {
                            Console.WriteLine($"{employee.full_name}");
                            Console.WriteLine(eDepartment.id.ToString());
                            departments.Add(eDepartment.department.name);
                        }

                        foreach(var ePosition in employee.EmployeePositions)
                        {
                            positions.Add(ePosition.position.name);
                        }

                        workbookpart?.InsertCell($"A{row}", $"{index + 1}", DataType.NUMBER);
                        workbookpart?.InsertCell($"B{row}", $"{employee.employee_code}", DataType.TEXT);
                        workbookpart?.InsertCell($"C{row}", $"{employee.full_name}", DataType.TEXT);
                        workbookpart?.InsertCell($"D{row}", $"{employee.initial_name}", DataType.TEXT);
                        workbookpart?.InsertCell($"E{row}", $"{employee.branch}", DataType.TEXT);
                        workbookpart?.InsertCell($"F{row}", $"{String.Join(", ", departments.ToArray())}", DataType.TEXT);
                        workbookpart?.InsertCell($"G{row}", $"{String.Join(", ", positions.ToArray())}", DataType.TEXT);
                        workbookpart?.InsertCell($"H{row}", $"{states[employee.state]}", DataType.TEXT);
                        workbookpart?.InsertCell($"I{row}", $"{employee.phone}\u200B", DataType.TEXT);
                        workbookpart?.InsertCell($"J{row}", $"{employee.company_email}", DataType.TEXT);
                        workbookpart?.InsertCell($"K{row}", $"{employee.personal_email}", DataType.TEXT);
                        workbookpart?.InsertCell($"L{row}", $"{employee.birthday.ToString("dd/MM/yyyy")}", DataType.TEXT);
                        workbookpart?.InsertCell($"M{row}", $"{employee.permanent_address}", DataType.TEXT);
                        workbookpart?.InsertCell($"N{row}", $"{employee.current_address}", DataType.TEXT);
                        workbookpart?.InsertCell($"O{row}", $"{employee.id_number}\u200B", DataType.TEXT);
                        workbookpart?.InsertCell($"P{row}", $"{employee.date_issue?.ToString("dd/MM/yyyy")}", DataType.TEXT);
                        workbookpart?.InsertCell($"Q{row}", $"{employee.location_issue}", DataType.TEXT);
                        workbookpart?.InsertCell($"R{row}", $"{(employee.is_married ? "Đã kết hôn" : "Độc thân")}", DataType.TEXT);
                    }
                }
                return ms.ToArray();
            }
        }

        public async Task<byte[]> ExportTemplateExcel()
        {
            using (var ms = new MemoryStream())
            {
                await AddFileToMemoryStream(ms, _templatePath);
                return ms.ToArray();
            }
        }


        #region private


        private async Task<PagedList<EmployeeResponse>> GetAll()
        {
            RequestEmployeePaged req = new RequestEmployeePaged() { 
                sort = "employee_code.asc",
                size = 9999
            };

            var data = await GetPaged(req);

            return data;
        }

        private async Task AddFileToMemoryStream(MemoryStream ms, string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                fileStream.CopyTo(ms);
                ms.Position = 0;
            }
        }

        #endregion
    }
}
