using AutoMapper;
using Framework.Core.Collections;
using Framework.Core.Extensions;
using Application.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Common.Abstractions;
using Application.Core.Interfaces.Core;

namespace Application.Core.Services.Core
{
    public class RoleServices : BaseService, IRoleServices
    {
        private readonly IRepository<Role> roleRepository;
        private readonly IRepository<Permission> permissionRepository;

        public RoleServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper)
        {
            roleRepository = _unitOfWork.GetRepository<Role>();
            permissionRepository = _unitOfWork.GetRepository<Permission>();
        }

        public async Task<PagedList<RoleResponse>> GetPaged(PagedRequest request)
        {
            var data = roleRepository
                                  .GetQuery()
                                  .ExcludeSoftDeleted()
                                  .Where(x => !string.IsNullOrEmpty(request.search) ? x.code.ToLower().Contains(request.search.ToLower()) || x.name.ToLower().Contains(request.search.ToLower()) : true)
                                  .Include(x => x.permissions.Where(y => y.del_flg == false))
                                  .ThenInclude(x => x.function)
                                  .SortBy(request.sort)
                                  .ToPagedList(request.page, request.size);

            var dataMapping = _mapper.Map<PagedList<RoleResponse>>(data);
            return dataMapping;
        }

        public async Task<RoleResponse> GetById(Guid id)
        {
            var role = await roleRepository
                                  .GetQuery()
                                  .ExcludeSoftDeleted()
                                  .FilterById(id)
                                  .Include(x => x.permissions.Where(y => y.del_flg == false))
                                  .ThenInclude(x => x.function)
                                  .FirstOrDefaultAsync();

            var data = _mapper.Map<RoleResponse>(role);
            return data;
        }

        public async Task<int> Create(RoleRequest request)
        {
            var role = _mapper.Map<Role>(request);
            await roleRepository.AddEntityAsync(role);
            await ConfigPermission(role.code, request.permissions);
            var count = await _unitOfWork.SaveChangesAsync();

            return count;
        }

        public async Task<int> Update(Guid id, RoleRequest request)
        {
            var count = 0;
            var role = _unitOfWork
                            .GetRepository<Role>()
                            .GetQuery()
                            .FindActiveById(id)
                            .FirstOrDefault();

            if (role == null)
                return count;

            _mapper.Map(request, role);
            await roleRepository.UpdateEntityAsync(role);
            await ConfigPermission(role.code, request.permissions);
            count = await _unitOfWork.SaveChangesAsync();
            return count;

        }

        public async Task<int> Delete(Guid id)
        {
            var role = roleRepository.GetQuery().FindActiveById(id).FirstOrDefault();

            if (role == null)
                return 0;

            await roleRepository.DeleteEntityAsync(role);
            var count = await _unitOfWork.SaveChangesAsync();
            return count;
        }

        public async Task<int> Delete(Guid[] ids)
        {
            var role = roleRepository.GetQuery().FilterByIds(ids).ToList();

            foreach (var item in role)
            {
                await roleRepository.DeleteEntityAsync(item);
            }
            var count = await _unitOfWork.SaveChangesAsync();
            return count;
        }

        private async Task ConfigPermission(string code, List<string> request)
        {
            // insert new
            var permissions = _unitOfWork.GetRepository<Function>().GetQuery().ExcludeSoftDeleted().Where(x => request.Contains(x.code)).ToArray();
            var permission_cds = permissions.Select(y => y.code).ToArray();
            // delete old role
            var oldPermission = permissionRepository.GetQuery().Where(x => x.role_cd == code).ToArray();

            foreach (var item in oldPermission)
            {
                if (permission_cds.Contains(item.function_cd))
                {
                    item.del_flg = false;
                    await permissionRepository.UpdateEntityAsync(item);
                }
                else
                    await permissionRepository.DeleteEntityAsync(item);
            }

            var newPermission = permissions.Where(x => !oldPermission.Select(y => y.function_cd).ToArray().Contains(x.code)).ToArray();
            foreach (var item in newPermission)
            {
                var newEntity = new Permission() { role_cd = code, function_cd = item.code };
                await permissionRepository.AddEntityAsync(newEntity);
            }
        }
    }
}
