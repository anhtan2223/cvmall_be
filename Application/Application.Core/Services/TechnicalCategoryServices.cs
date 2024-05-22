using AutoMapper;
using Framework.Core.Extensions;
using Domain.Entities;
using Application.Common.Abstractions;
using Application.Core.Interfaces.Core;
using Application.Core.Contracts;
using Framework.Core.Collections;
using Microsoft.EntityFrameworkCore;

namespace Application.Core.Services.Core
{
    public class TechnicalCategoryServices : BaseService, ITechnicalCategoryServices
    {
        private readonly IRepository<TechnicalCategory> technicalCategoryRepository;

        public TechnicalCategoryServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper)
        {
            technicalCategoryRepository = _unitOfWork.GetRepository<TechnicalCategory>();
        }

        public async Task<IList<TechnicalCategoryResponse>> GetList()
        {
            var data = await technicalCategoryRepository
                    .GetQuery()
                    .ExcludeSoftDeleted()
                    .SortBy("updated_at.desc").ToPagedListAsync(1, 9999);

            List<TechnicalCategoryResponse> dataMapping = new();

            if (data?.data?.Count > 0)
            {
                dataMapping = _mapper.Map<IList<TechnicalCategory>, List<TechnicalCategoryResponse>>(data.data);
            }

            return dataMapping;
        }

        public async Task<PagedList<TechnicalCategoryResponse>> GetPaged(RequestPaged request)
        {
            var data = await technicalCategoryRepository
                    .GetQuery()
                    .ExcludeSoftDeleted()
                    .Where(x => string.IsNullOrEmpty(request.search) || x.Name.ToLower().Contains(request.search.ToLower()))
                    .SortBy("updated_at.desc")
                    .Include(x => x.Technicals)
                    .ToPagedListAsync(request.page, request.size);

            if (data?.data?.Count > 0)
            {
                foreach (var item in data.data)
                {
                    if (item.Technicals?.Count > 0)
                    {
                        foreach (var childItem in item.Technicals)
                        {
                            if (childItem.del_flg)
                            {
                                item.Technicals.Remove(childItem);
                            }
                        }
                    }
                }
            }

            var dataMapping = _mapper.Map<PagedList<TechnicalCategoryResponse>>(data);

            return dataMapping;
        }

        public async Task<TechnicalCategoryResponse> GetById(Guid id)
        {
            var entity = technicalCategoryRepository
                                  .GetQuery()
                                  .ExcludeSoftDeleted()
                                  .FilterById(id)
                                  .FirstOrDefault();

            var data = _mapper.Map<TechnicalCategoryResponse>(entity);

            return data;
        }

        public async Task<int> Create(TechnicalCategoryRequest request)
        {
            var tech_cat = _mapper.Map<TechnicalCategory>(request);

            await technicalCategoryRepository.AddEntityAsync(tech_cat);

            var count = await _unitOfWork.SaveChangesAsync();

            return count;
        }

        public async Task<int> Update(Guid id, TechnicalCategoryRequest request)
        {
            var count = 0;
            var entity = _unitOfWork
                            .GetRepository<TechnicalCategory>()
                            .GetQuery()
                            .FindActiveById(id)
                            .FirstOrDefault();

            if (entity == null)
                return count;

            _mapper.Map(request, entity);
            await technicalCategoryRepository.UpdateEntityAsync(entity);

            count = await _unitOfWork.SaveChangesAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var entity = technicalCategoryRepository.GetQuery().FindActiveById(id).FirstOrDefault();

            if (entity == null)
                return 0;

            await technicalCategoryRepository.DeleteEntityAsync(entity);
            var count = await _unitOfWork.SaveChangesAsync();
            return count;
        }

    }
}
