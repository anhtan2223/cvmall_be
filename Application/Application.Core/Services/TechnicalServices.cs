using AutoMapper;
using Framework.Core.Extensions;
using Domain.Entities;
using Application.Common.Abstractions;
using Application.Core.Interfaces.Core;
using Application.Core.Contracts;
using Framework.Core.Collections;

namespace Application.Core.Services.Core
{
    public class TechnicalServices : BaseService, ITechnicalServices
    {
        private readonly IRepository<Technical> technicalRepository;

        public TechnicalServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper)
        {
            technicalRepository = _unitOfWork.GetRepository<Technical>();
        }

        public async Task<IList<TechnicalResponse>> GetList()
        {
            var data = await technicalRepository
                    .GetQuery()
                    .ExcludeSoftDeleted()
                    .SortBy("updated_at.desc").ToPagedListAsync(1, 9999);

            List<TechnicalResponse> dataMapping = new();

            if (data?.data?.Count > 0)
            {
                dataMapping = _mapper.Map<IList<Technical>, List<TechnicalResponse>>(data.data);
            }

            return dataMapping;
        }

        public async Task<PagedList<TechnicalResponse>> GetPaged(RequestPaged request)
        {
            var data = await technicalRepository
                    .GetQuery()
                    .ExcludeSoftDeleted()
                    .Where(x => string.IsNullOrEmpty(request.search) || x.Name.ToLower().Contains(request.search.ToLower()))
                    .SortBy("updated_at.desc")
                    .ToPagedListAsync(request.page, request.size);

            var dataMapping = _mapper.Map<PagedList<TechnicalResponse>>(data);

            return dataMapping;
        }

        public async Task<TechnicalResponse> GetById(Guid id)
        {
            var entity = technicalRepository
                                  .GetQuery()
                                  .ExcludeSoftDeleted()
                                  .FilterById(id)
                                  .FirstOrDefault();

            var data = _mapper.Map<TechnicalResponse>(entity);

            return data;
        }

        public async Task<int> Create(TechnicalRequest request)
        {
            var count = 0;

            var technical = _mapper.Map<Technical>(request);

            var tech_cat = _unitOfWork
                            .GetRepository<TechnicalCategory>()
                            .GetQuery()
                            .FindActiveById(request.TechnicalCategoryId)
                            .FirstOrDefault();

            if(tech_cat == null)
            {
                return count;
            }

            technical.TechnicalCategory = tech_cat;

            await technicalRepository.AddEntityAsync(technical);

            count = await _unitOfWork.SaveChangesAsync();

            return count;
        }

        public async Task<int> Update(Guid id, TechnicalRequest request)
        {
            var count = 0;

            var entity = _unitOfWork
                            .GetRepository<Technical>()
                            .GetQuery()
                            .FindActiveById(id)
                            .FirstOrDefault();

            if (entity == null)
                return count;

            _mapper.Map(request, entity);
            await technicalRepository.UpdateEntityAsync(entity);

            count = await _unitOfWork.SaveChangesAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var entity = technicalRepository.GetQuery().FindActiveById(id).FirstOrDefault();

            if (entity == null)
                return 0;

            await technicalRepository.DeleteEntityAsync(entity);
            var count = await _unitOfWork.SaveChangesAsync();
            return count;
        }
    }
}
