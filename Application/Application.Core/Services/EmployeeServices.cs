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
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using Framework.Core.Helpers;
using Framework.Core.Helpers.Excel;
using Application.Common.Extensions;
using Microsoft.Extensions.Hosting;


namespace Application.Core.Services.Core
{
    public class EmployeeServices : BaseService, IEmployeeServices
    {
        private readonly IRepository<Employee> employeeRepository;
        private ILocalizeServices ls { get; set; }
        private string _templatePath;


        public EmployeeServices(IUnitOfWork _unitOfWork, IMapper _mapper, ILocalizeServices _ls, IHostEnvironment _env) : base(_unitOfWork, _mapper)
        {
            employeeRepository = _unitOfWork.GetRepository<Employee>();
            ls = _ls;
            _templatePath = Path.Combine(_env.ContentRootPath, "wwwroot", "Assets", "Employee", "Employee_Template.xlsx");
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
                                    .ExcludeSoftDeleted()
                                    .AnyAsync(x => x.employee_code == employeeCode);
        }

        public async Task<int> Create(EmployeeRequest request)
        {
            var count = 0;

            string pattern = @"^VHEC-\d{4}$";
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

            string pattern = @"^VHEC-\d{4}$";
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
                            departments.Add(eDepartment.department.name);
                        }

                        foreach(var ePosition in employee.EmployeePositions)
                        {
                            positions.Add(ePosition.position.name);
                        }

                        workbookpart?.InsertCell($"A{row}", $"{index + 1}", DataType.NUMBER);
                        if (employee.employee_code != null) workbookpart?.InsertCell($"B{row}", $"{employee.employee_code}", DataType.TEXT);
                        if (employee.full_name != null) workbookpart?.InsertCell($"C{row}", $"{employee.full_name}", DataType.TEXT);
                        if (employee.initial_name != null) workbookpart?.InsertCell($"D{row}", $"{employee.initial_name}", DataType.TEXT);
                        if (employee.branch != null) workbookpart?.InsertCell($"E{row}", $"{employee.branch}", DataType.TEXT);
                        if (departments.Count() > 0) workbookpart?.InsertCell($"F{row}", $"{String.Join(", ", departments.ToArray())}", DataType.TEXT);
                        if (positions.Count() > 0) workbookpart?.InsertCell($"G{row}", $"{String.Join(", ", positions.ToArray())}", DataType.TEXT);
                        if (employee.state != null) workbookpart?.InsertCell($"H{row}", $"{states[employee.state]}", DataType.TEXT);
                        if (employee.phone != null) workbookpart?.InsertCell($"I{row}", $"{employee.phone}", DataType.TEXT);
                        if (employee.company_email != null) workbookpart?.InsertCell($"J{row}", $"{employee.company_email}", DataType.TEXT);
                        if (employee.personal_email != null) workbookpart?.InsertCell($"K{row}", $"{employee.personal_email}", DataType.TEXT);
                        if (employee.birthday != null) workbookpart?.InsertCell($"L{row}", $"{employee.birthday.ToString("dd/MM/yyyy")}", DataType.TEXT);
                        if (employee.permanent_address != null) workbookpart?.InsertCell($"M{row}", $"{employee.permanent_address}", DataType.TEXT);
                        if (employee.current_address != null) workbookpart?.InsertCell($"N{row}", $"{employee.current_address}", DataType.TEXT);
                        if (employee.id_number != null) workbookpart?.InsertCell($"O{row}", $"{employee.id_number}", DataType.TEXT);
                        if (employee.date_issue != null) workbookpart?.InsertCell($"P{row}", $"{employee.date_issue?.ToString("dd/MM/yyyy")}", DataType.TEXT);
                        if (employee.location_issue != null) workbookpart?.InsertCell($"Q{row}", $"{employee.location_issue}", DataType.TEXT);
                        if (employee.is_married  != null) workbookpart?.InsertCell($"R{row}", $"{(employee.is_married ? "Đã kết hôn" : "Độc thân")}", DataType.TEXT);
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
        public async Task<string> Import(IFormFile file)
        {
            var count = 0;
            var message = ls.Get(Modules.Core, Screen.Message, MessageKey.I_001) ;
            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    List<string> fieldName = new List<string>{
                        "STT " , "Employee Code " , "Name " , "Initial Name " , "Branch " ,
                        "Department " , "Position " , "State " , "Phone " ,
                        "Company Email " , "Personal Email " , "BirthDay " ,
                        "Permanet Address " , "Current Address " , "ID Number " ,
                        "Date Issue " , "Location Issue " , "Married State "
                    };
                    Dictionary<string, int> stateMappings = new Dictionary<string, int>
                    {
                        { "đang làm việc", 0 },
                        { "đang thử việc", 1 },
                        { "đang thực tập", 2 },
                        { "đã nghỉ việc", 3 }
                    };
                    Dictionary<string, bool> marriedMappings = new Dictionary<string, bool>
                    {
                        { "độc thân", false },
                        { "đã kết hôn", true }
                    };
                    List<string> branchs = new List<string>{
                        "cần thơ" , "nha trang" , "hà nội" , "japan"
                    };
                    string pattern = @"^VHEC-\d{4}$";
                    Regex regex = new Regex(pattern);
                    List<List<string>> excelData = ReadExcelData(file);
                    foreach (var employee in excelData)
                    {
                        if(employee.Count < 18 || employee[0] == "STT" || employee.All(x => x==null))
                            continue ;
                        
                        count++ ;
                        
                        foreach (var item in new List<int>{1,2,3,4,7,8,9,11,14})
                        {
                            if(employee[item] == null)
                                throw new ArgumentException(fieldName[item] + " Not Null");
                        }

                        var isValidEmployeeCode = !await employeeRepository
                                                    .GetQuery()
                                                    .ExcludeSoftDeleted()
                                                    .AnyAsync(x => x.employee_code == employee[1]);                          
                        if(!isValidEmployeeCode)
                            throw new ArgumentException("Employee Code Already Exists");

                        if(!regex.IsMatch(employee[1]))
                            throw new ArgumentException("Employee Code Invalid Value ");
                        if(!branchs.Contains(employee[4].ToLower()))
                            throw new ArgumentException(fieldName[4]+ employee[4].ToLower() + " Is Invalid Value");
                        if(!stateMappings.ContainsKey(employee[7].ToLower()))
                            throw new ArgumentException(fieldName[7]+ employee[7] + " Is Invalid Value");
                        if(!Regex.IsMatch(employee[8] , @"^\d+$"))
                            throw new ArgumentException(fieldName[8]+ employee[8] + " Is Invalid Value");
                        if(!Regex.IsMatch(employee[9] , @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                            throw new ArgumentException(fieldName[9]+ employee[9] + " Is Invalid Value");
                        if(employee[10]!=null && !Regex.IsMatch(employee[10] , @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                            throw new ArgumentException(fieldName[10]+ employee[10] + " Is Invalid Value");
                        if(!Regex.IsMatch(employee[14] , @"^\d+$"))
                            throw new ArgumentException(fieldName[14]+ employee[14] + " Is Invalid Value");
                        if(!marriedMappings.ContainsKey(employee[17].ToLower()))
                            throw new ArgumentException(fieldName[17]+ employee[17] + " Is Invalid Value");
                        
                        
                        var request = new EmployeeRequest{
                            employee_code       = employee[1] ,
                            full_name           = employee[2] ,
                            initial_name        = employee[3] ,
                            branch              = employee[4] ,
                            current_group       = ""          ,
                            EmployeeDepartments = (employee[5] == null) ? null : GetDepartmentFromString(employee[5]) ,
                            EmployeePositions   = (employee[6] == null) ? null : GetPositionFromString(employee[6]) ,
                            state               = stateMappings[employee[7].ToLower()] ,
                            phone               = employee[8] ,
                            company_email       = employee[9] ,
                            personal_email      = employee[10] ,
                            birthday            = StringToDateTime(employee[11]) ,
                            permanent_address   = employee[12] ,
                            current_address     = employee[13] ,
                            id_number           = employee[14] ,
                            date_issue          = (employee[15] == null) ? null : StringToDateTime(employee[15]) ,
                            location_issue      = employee[16] ,
                            is_married          = marriedMappings[employee[17].ToLower()]
                        };
                        var Employee = _mapper.Map<Employee>(request);
                        await employeeRepository.AddEntityAsync(Employee) ;
                        await _unitOfWork.SaveChangesAsync();
                    }
                    
                    transaction.Commit();
                    count = 0 ;
                }
                catch (Exception ex)
                {
                    message = ex.Message ;
                    transaction.Rollback();
                }
            }
            if(count == 0)
                return ls.Get(Modules.Core, Screen.Message, MessageKey.I_001);
            return "Error At Row " +count + ": "+ message ;
        }
        private DateTime StringToDateTime(string input)
        {
            long dateType ;
            if(long.TryParse(input ,out dateType))
                return new DateTime(1900,1,1).AddDays(dateType - 2);

           return DateTime.ParseExact(input,"dd/MM/yyyy",CultureInfo.InvariantCulture);
        }
        private List<EmployeePositionRequest> GetPositionFromString(string input)
        {
            var result = new List<EmployeePositionRequest>() ;
            var list  = input.Split(",").Select(x => x.ToLower()).ToList();
            var position = _unitOfWork
                                .GetRepository<Position>()
                                .GetQuery()
                                .ExcludeSoftDeleted()
                                .Where(x => list.Contains(x.name.ToLower()));
            foreach (var p in position)
            {
                result.Add(new EmployeePositionRequest{
                    position_id = p.id 
                });
            }
            return result ;
        }
        private List<EmployeeDepartmentRequest> GetDepartmentFromString(string input)
        {
            var result = new List<EmployeeDepartmentRequest>() ;
            var list  = input.Split(",").Select(x => x.ToLower()).ToList();
            var department = _unitOfWork
                                .GetRepository<Department>()
                                .GetQuery()
                                .ExcludeSoftDeleted()
                                .Where(x => list.Contains(x.name.ToLower()));
            foreach (var d in department)
            {
                result.Add(new EmployeeDepartmentRequest{
                    department_id = d.id 
                });
            }
            return result ;
        }
        private List<List<string>> ReadExcelData(IFormFile file)
        {
            List<List<string>> data = new List<List<string>>();

            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                stream.Position = 0;

                using (SpreadsheetDocument document = SpreadsheetDocument.Open(stream, false))
                {
                    WorkbookPart workbookPart = document.WorkbookPart;
                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                    SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                    foreach (Row row in sheetData.Elements<Row>())
                    {
                        List<string> rowData = new List<string>();
                        foreach (Cell cell in row.Elements<Cell>())
                        {
                            rowData.Add(GetCellValue(document, cell));
                        }
                        data.Add(rowData);
                    }
                }
            }
            return data;
        }
        private string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            string value = cell.CellValue?.InnerText;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                return document.WorkbookPart.SharedStringTablePart.SharedStringTable
                    .Elements<SharedStringItem>().ElementAt(int.Parse(value)).InnerText;
            }
            return value;
        }
    }
}
