using AutoMapper;
using Framework.Core.Collections;
using Framework.Core.Extensions;
using Application.Core.Contracts;
using Domain.Entities;
using Application.Common.Abstractions;
using Application.Core.Interfaces.Core;
using Microsoft.EntityFrameworkCore;

namespace Application.Core.Services.Core
{
    public class CvInfoServices : BaseService, ICvInfoServices
    {
        private readonly IRepository<CvInfo> cvInfoRepository;

        public CvInfoServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper)
        {
            cvInfoRepository = _unitOfWork.GetRepository<CvInfo>();
        }

        public async Task<PagedList<CvInfoResponse>> GetPaged(RequestPaged request)
        {
            var cvInfos = await cvInfoRepository
                    .GetQuery()
                    .ExcludeSoftDeleted()
                    .Where(x => string.IsNullOrEmpty(request.search) || x.name.ToLower().Contains(request.search.ToLower()))
                    .SortBy(request.sort ?? "updated_at.desc")
                    .Include(x => x.cvTechInfos)
                    .Include(y => y.bizInfos)
                    .ToPagedListAsync(request.page, request.size);

            var dataMapping = _mapper.Map<PagedList<CvInfoResponse>>(cvInfos);

            return dataMapping;
        }

        public async Task<IList<CvInfoResponse>> GetList() 
        {
            var data = await cvInfoRepository
                    .GetQuery()
                    .ExcludeSoftDeleted()
                    .SortBy("updated_at.desc").ToPagedListAsync(1, 9999);

            List<CvInfoResponse> dataMapping = new();

            if (data?.data?.Count > 0)
            {
                dataMapping = _mapper.Map<IList<CvInfo>, List<CvInfoResponse>>(data.data);
            }

            return dataMapping;
        }

        public async Task<CvInfoResponse> GetById(Guid id)
        {
            var entity = cvInfoRepository
                                  .GetQuery()
                                  .ExcludeSoftDeleted()
                                  .FilterById(id)
                                  .Include(x => x.cvTechInfos)
                                  .Include(x => x.bizInfos)
                                  .FirstOrDefault();

            var data = _mapper.Map<CvInfoResponse>(entity);

            return data;
        }

        public async Task<int> Create(CvInfoRequest request)
        {
            var count = 0;

            var cvInfo = _mapper.Map<CvInfo>(request);

            await cvInfoRepository.AddEntityAsync(cvInfo);

            count = await _unitOfWork.SaveChangesAsync();

            return count;
        }

        public async Task<int> Update(Guid id, CvInfoRequest request)
        {
            var count = 0;

            var entity = _unitOfWork
                            .GetRepository<CvInfo>()
                            .GetQuery()
                            .FindActiveById(id)
                            .FirstOrDefault();

            if (entity == null)
                return count;

            _mapper.Map(request, entity);
            await cvInfoRepository.UpdateEntityAsync(entity);

            count = await _unitOfWork.SaveChangesAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var entity = cvInfoRepository.GetQuery().FindActiveById(id).FirstOrDefault();

            if (entity == null)
                return 0;

            await cvInfoRepository.DeleteEntityAsync(entity);
            var count = await _unitOfWork.SaveChangesAsync();
            return count;
        }
    }
}
