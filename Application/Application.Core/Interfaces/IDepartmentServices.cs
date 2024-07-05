using Application.Core.Contracts;
using Application.Core.Contracts.Department;
using Domain.Entities;
using Framework.Core.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.Interfaces.Core
{
    public interface IDepartmentServices
    {
        Task<IList<DepartmentResponse>> GetAll();
        Task<int> Create(DepartmentRequest newDepartment);
        Task<int> Update(Guid id, DepartmentRequest updateDepartment);
        Task<int> Delete(Guid id);
    }
}
