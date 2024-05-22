using AutoMapper;
using Framework.Core.Collections;
using Framework.Core.Extensions;
using Application.Core.Contracts;
using Domain.Entities;
using Application.Common.Abstractions;
using Application.Core.Interfaces.Core;

namespace Application.Core.Services.Core
{
    public class BizInfoServices : BaseService, IBizInfoServices
    {
        private readonly IRepository<BizInfo> bizInfoRepository;

        public BizInfoServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper)
        {
            bizInfoRepository = _unitOfWork.GetRepository<BizInfo>();
        }

        public async Task<PagedList<BizInfoResponse>> GetPaged(RequestPaged request)
        {
            var data = await bizInfoRepository
                    .GetQuery()
                    .ExcludeSoftDeleted()
                    .SortBy(request.sort ?? "updated_at.desc")
                    .ToPagedListAsync(request.page, request.size);

            var dataMapping = _mapper.Map<PagedList<BizInfoResponse>>(data);

            return dataMapping;
        }

        public async Task<IList<BizInfoResponse>> GetList()
        {
            var data = await bizInfoRepository
                .GetQuery()
                .ExcludeSoftDeleted()
                .SortBy("updated_at.desc").ToPagedListAsync(1, 9999);

            List<BizInfoResponse> dataMapping = new();

            if (data?.data?.Count > 0)
            {
                dataMapping = _mapper.Map<IList<BizInfo>, List<BizInfoResponse>>(data.data);
            }

            return dataMapping;
        }

        public async Task<BizInfoResponse> GetById(Guid id)
        {
            var entity = bizInfoRepository
                                  .GetQuery()
                                  .ExcludeSoftDeleted()
                                  .FilterById(id)
                                  .FirstOrDefault();

            var data = _mapper.Map<BizInfoResponse>(entity);

            return data;
        }

        public async Task<int> Create(BizInfoRequest request)
        {
            var count = 0;

            var cvInfo = _unitOfWork.GetRepository<CvInfo>()
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FindActiveById(request.cvInfoId)
                                    .FirstOrDefault();
            if (cvInfo == null)
            {
                return count;
            }

            var bizInfo = _mapper.Map<BizInfo>(request);

            await bizInfoRepository.AddEntityAsync(bizInfo);

            count = await _unitOfWork.SaveChangesAsync();

            return count;
        }

        public async Task<int> Update(Guid id, BizInfoRequest request)
        {
            var count = 0;

            var entity = _unitOfWork
                            .GetRepository<BizInfo>()
                            .GetQuery()
                            .FindActiveById(id)
                            .FirstOrDefault();

            if (entity == null)
                return count;

            request.cvInfoId = entity.cvInfoId;

            _mapper.Map(request, entity);
            await bizInfoRepository.UpdateEntityAsync(entity);

            count = await _unitOfWork.SaveChangesAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var entity = bizInfoRepository.GetQuery().FindActiveById(id).FirstOrDefault();

            if (entity == null)
                return 0;

            await bizInfoRepository.DeleteEntityAsync(entity);
            var count = await _unitOfWork.SaveChangesAsync();
            return count;
        }
    }
}
