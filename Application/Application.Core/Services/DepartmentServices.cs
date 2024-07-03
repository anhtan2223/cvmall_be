using Application.Common.Abstractions;
using Application.Core.Contracts;
using Application.Core.Contracts.Department;
using Application.Core.Interfaces.Core;
using AutoMapper;
using Domain.Entities;
using Framework.Core.Extensions;
using System.Data.Entity;

namespace Application.Core.Services.Core
{
    public class DepartmentServices : BaseService, IDepartmentServices
    {
        private readonly IRepository<Department> departmentRepository;
        
        public DepartmentServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base (_unitOfWork, _mapper)
        {
            departmentRepository = _unitOfWork.GetRepository<Department>();
        }

        public async Task<int> Create(DepartmentRequest newDepartment)
        {
            var count = 0;
            bool isExisted = CheckExistedDeparment(newDepartment);
            if (!isExisted)
            {
                var department = _mapper.Map<Department>(newDepartment);

                await departmentRepository.AddEntityAsync(department);
                
                count += await _unitOfWork.SaveChangesAsync();
            }

            return count;
        }

        public async Task<IList<DepartmentResponse>> GetAll()
        {
            var data = await departmentRepository
                    .GetQuery()
                    .ExcludeSoftDeleted()
                    .ToPagedListAsync(1, 9999);

            List<DepartmentResponse> dataMapping = new();

            if (data?.data?.Count > 0)
            {
                dataMapping = _mapper.Map<IList<Department>, List<DepartmentResponse>>(data.data);
            }

            return dataMapping;
        }

        private bool CheckExistedDeparment(DepartmentRequest department)
        {
            return departmentRepository.GetQuery().Where(x => x.name == department.name).Any();
        }
    }
}
