using AutoMapper;
using Framework.Core.Abstractions;
using Framework.Core.Collections;
using Framework.Core.Extensions;
using Application.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Common.Abstractions;
using Application.Core.Interfaces.Core;

namespace Application.Core.Services.Core
{
    public class FunctionServices : BaseService, IFunctionServices
    {
        private readonly IRepository<Function> functionRepository;
        private readonly IRepository<User> userRepository;


        public FunctionServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper)
        {
            functionRepository = _unitOfWork.GetRepository<Function>();
        }

        public async Task<PagedList<FunctionResponse>> GetPaged(FunctionPagedRequest request)
        {
            var data = functionRepository.GetQuery().ExcludeSoftDeleted()
                .Include(x => x.parent)
                .Where(x => (!string.IsNullOrEmpty(request.module) ? x.module.Contains(request.module) : true)
                  && (!string.IsNullOrEmpty(request.parent) ? x.parent != null && x.parent.code.Contains(request.parent) : true)
                  && (!string.IsNullOrEmpty(request.name) ? x.name.Contains(request.name) || x.description.Contains(request.name) || x.code.Contains(request.name) : true)
                  && (!string.IsNullOrEmpty(request.code) ? x.code.Contains(request.code) : true))
                .SortBy(request.sort)
                .ToPagedList(request.page, request.size);

            var dataMapping = _mapper.Map<PagedList<FunctionResponse>>(data);
            return dataMapping;
        }

        public async Task<FunctionResponse> GetById(Guid id)
        {
            var entity = functionRepository
                                  .GetQuery()
                                  .ExcludeSoftDeleted()
                                  .FilterById(id)
                                  .FirstOrDefault();

            var data = _mapper.Map<FunctionResponse>(entity);
            return data;
        }

        public async Task<int> Create(FunctionRequest request)
        {
            var entity = _mapper.Map<Function>(request);
            await functionRepository.AddEntityAsync(entity);
            var count = await _unitOfWork.SaveChangesAsync();

            return count;
        }

        public async Task<int> Update(Guid id, FunctionRequest request)
        {
            var count = 0;
            var entity = _unitOfWork
                            .GetRepository<Function>()
                            .GetQuery()
                            .FindActiveById(id)
                            .FirstOrDefault();

            if (entity == null)
                return count;

            _mapper.Map(request, entity);
            await functionRepository.UpdateEntityAsync(entity);
            count = await _unitOfWork.SaveChangesAsync();
            return count;

        }

        public async Task<int> Delete(Guid id)
        {
            var entity = functionRepository.GetQuery().FindActiveById(id).FirstOrDefault();

            if (entity == null)
                return 0;

            await functionRepository.DeleteEntityAsync(entity);
            var count = await _unitOfWork.SaveChangesAsync();
            return count;
        }

        public async Task<int> Delete(Guid[] ids)
        {
            var entitys = functionRepository.GetQuery().FilterByIds(ids).ToList();

            foreach (var item in entitys)
            {
                await functionRepository.DeleteEntityAsync(item);
            }
            var count = await _unitOfWork.SaveChangesAsync();
            return count;
        }

        public async Task<int> ChangeIsActive(Guid[] ids)
        {
            var entitys = functionRepository.GetQuery().FilterByIds(ids).ToList();

            foreach (var item in entitys)
            {
                item.is_active = true;
                await functionRepository.UpdateEntityAsync(item);
            }
            var count = await _unitOfWork.SaveChangesAsync();
            return count;
        }

        public async Task<List<FunctionResponse>> GetAsTreeView()
        {
            List<FunctionResponse> response = new List<FunctionResponse>();

            var listFunction = functionRepository.GetQuery().ExcludeSoftDeleted().ToList();
            var listParent = listFunction.Where(x => x.parent_cd == null).Distinct().OrderBy(x => x.order).ToList();

            response.AddRange(_mapper.Map<List<FunctionResponse>>(listParent));

            foreach (var item in response)
            {

                var listScreen = listFunction.Where(x => x.parent_cd == item.code).OrderBy(x => x.order).ToArray();
                if (listScreen != null && listScreen.Count() > 0)
                {
                    item.items = new List<FunctionResponse>();
                    item.items.AddRange(_mapper.Map<List<FunctionResponse>>(listScreen));
                    foreach (var childItem in item.items)
                    {

                        var listApi = listFunction.Where(x => x.parent_cd == childItem.code).OrderBy(x => x.order).ToArray();
                        if (listApi != null && listApi.Count() > 0)
                        {
                            childItem.items = new List<FunctionResponse>();
                            childItem.items.AddRange(_mapper.Map<List<FunctionResponse>>(listApi));
                        }
                    }
                }
            }

            return response;
        }
    }
}
