using AutoMapper;
using Framework.Core.Collections;
using Framework.Core.Extensions;
using Application.Core.Contracts;
using Domain.Entities;
using Application.Common.Abstractions;
using Application.Core.Interfaces.Core;
// using System.Data.Entity;
using Microsoft.EntityFrameworkCore;


namespace Application.Core.Services.Core
{
    public class EmployeeServices : BaseService, IEmployeeServices
    {
        private readonly IRepository<Employee> employeeRepository;

        public EmployeeServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper)
        {
            employeeRepository = _unitOfWork.GetRepository<Employee>();
        }

        public async Task<PagedList<EmployeeResponse>> GetPaged(RequestEmployeePaged request)
        {

            var data = await employeeRepository
                    .GetQuery()
                    .ExcludeSoftDeleted()
                    .Where(x =>  ( request.state == null ) || request.state.Contains(x.state) )
                    .Where(x =>  ( request.group == null ) || request.group.Contains(x.current_group) )
                    .Where(x =>  ( request.branch == null ) || request.branch.Contains(x.branch) )
                    .Where(x =>  ( request.department == null ) || x.EmployeeDepartments.Any(y => request.department.Contains(y.Department.name) ) )
                    .Where(x =>  ( request.position == null ) || x.EmployeePositions.Any(y => request.position.Contains(y.Position.name) ) )
                    .Where(x =>  string.IsNullOrEmpty(request.search) || x.full_name.ToLower().Contains(request.search) || x.initial_name.ToLower().Contains(request.search)  )
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

            if(entity != null)
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

        public async Task<int> Create(EmployeeRequest request)
        {
            var count = 0;


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
    }
}
