using AutoMapper;
using Framework.Core.Extensions;
using Domain.Entities;
using Application.Common.Abstractions;
using Application.Core.Interfaces.Core;
using Application.Core.Contracts;
using Framework.Core.Collections;

namespace Application.Core.Services.Core
{
    public class CvTechnicalInfoServices : BaseService, ICvTechnicalInfoServices
    {
        private readonly IRepository<CvTechnicalInfo> cvTechInfoRepository;

        public CvTechnicalInfoServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper)
        {
            cvTechInfoRepository = _unitOfWork.GetRepository<CvTechnicalInfo>();
        }

        public async Task<PagedList<CvTechInfoResponse>> GetPaged(RequestPaged request)
        {
            var data = await cvTechInfoRepository
                    .GetQuery()
                    .ExcludeSoftDeleted()
                    .SortBy(request.sort ?? "updated_at.desc")
                    .ToPagedListAsync(request.page, request.size);

            var dataMapping = _mapper.Map<PagedList<CvTechInfoResponse>>(data);

            return dataMapping;
        }

        public async Task<IList<CvTechInfoResponse>> GetList()
        {
            var data = await cvTechInfoRepository
                    .GetQuery()
                    .ExcludeSoftDeleted()
                    .SortBy("updated_at.desc").ToPagedListAsync(1, 9999);

            List<CvTechInfoResponse> dataMapping = new();

            if (data?.data?.Count > 0)
            {
                dataMapping = _mapper.Map<IList<CvTechnicalInfo>, List<CvTechInfoResponse>>(data.data);
            }

            return dataMapping;
        }

        public async Task<CvTechInfoResponse> GetById(Guid id)
        {
            var entity = cvTechInfoRepository
                                  .GetQuery()
                                  .ExcludeSoftDeleted()
                                  .FilterById(id)
                                  .FirstOrDefault();

            var data = _mapper.Map<CvTechInfoResponse>(entity);

            return data;
        }

        public async Task<int> Create(CvTechnicalInfoRequest request)
        {
            var count = 0;

            var cvInfo = _unitOfWork.GetRepository<CvInfo>()
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FindActiveById(request.CvInfoId)
                                    .FirstOrDefault();
            if (cvInfo == null)
            {
                return count;
            }

            var technicalInfo = _unitOfWork.GetRepository<Technical>()
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FindActiveById(request.TechnicalId)
                                    .FirstOrDefault();

            if (technicalInfo == null)
            {
                return count;
            }

            request.CvInfoId = cvInfo.id;
            request.TechnicalId = technicalInfo.id;

            var bizInfo = _mapper.Map<CvTechnicalInfo>(request);

            await cvTechInfoRepository.AddEntityAsync(bizInfo);

            count = await _unitOfWork.SaveChangesAsync();

            return count;
        }

        public async Task<int> Update(Guid id, CvTechnicalInfoRequest request)
        {
            var count = 0;

            var entity = _unitOfWork
                            .GetRepository<CvTechnicalInfo>()
                            .GetQuery()
                            .FindActiveById(id)
                            .FirstOrDefault();

            if (entity == null)
                return count;

            _mapper.Map(request, entity);
            await cvTechInfoRepository.UpdateEntityAsync(entity);

            count = await _unitOfWork.SaveChangesAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var entity = cvTechInfoRepository.GetQuery().FindActiveById(id).FirstOrDefault();

            if (entity == null)
                return 0;

            await cvTechInfoRepository.DeleteEntityAsync(entity);
            var count = await _unitOfWork.SaveChangesAsync();
            return count;
        }
    }
}
