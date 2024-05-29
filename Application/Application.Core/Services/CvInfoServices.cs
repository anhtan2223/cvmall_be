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

            foreach (var cvInfo in cvInfos.data)
            {
                cvInfo.cvTechInfos = cvInfo?.cvTechInfos?.Where(cvTechInfo => !cvTechInfo.del_flg).ToList();
                cvInfo.bizInfos = cvInfo?.bizInfos?.Where(bizInfo => !bizInfo.del_flg).ToList();
            }

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
            if (entity != null)
            {
                entity.cvTechInfos = entity.cvTechInfos
                                           .Where(x => !x.del_flg)
                                           .ToList();
                entity.bizInfos = entity.bizInfos
                                           .Where(x => !x.del_flg)
                                           .ToList();
            }

            var data = _mapper.Map<CvInfoResponse>(entity);

            return data;
        }

        public async Task<int> Create(CvInfoRequest request)
        {
            var count = 0;

            request = ValidateRequest(request);

            var cvInfo = _mapper.Map<CvInfo>(request);

            await cvInfoRepository.AddEntityAsync(cvInfo);

            count += await _unitOfWork.SaveChangesAsync();

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

            var cvTechEntityList = _unitOfWork.GetRepository<CvTechnicalInfo>()
                                            .GetQuery()
                                            .ExcludeSoftDeleted()
                                            .Where(x => x.CvInfoId == id);

            var bizInfoEntityList = _unitOfWork.GetRepository<BizInfo>()
                                                .GetQuery()
                                                .ExcludeSoftDeleted()
                                                .Where(x => x.cvInfoId == id);

            var cvTechRequestList = request.cvTechInfos;

            var bizInfoRequestList = request.bizInfos;

            if (cvTechRequestList?.Count > 0)
            {
                // Delete cvtechnical
                foreach (var itemcvTechEntity in cvTechEntityList)
                {
                    var cvTechinRq = cvTechRequestList.Find(x => x.id == itemcvTechEntity.id);

                    if (cvTechinRq == null)
                    {
                        await _unitOfWork.GetRepository<CvTechnicalInfo>()
                                    .DeleteEntityAsync(itemcvTechEntity);
                    }
                }
            }
            else
            {
                foreach (var itemcvTechEntity in cvTechEntityList)
                {
                    await _unitOfWork.GetRepository<CvTechnicalInfo>()
                                    .DeleteEntityAsync(itemcvTechEntity);
                }
            }

            if (bizInfoRequestList?.Count > 0)
            {
                // Update or Delete biz info
                foreach (var itemBizEntity in bizInfoEntityList)
                {
                    var cvBizinRq = bizInfoRequestList.Find(x => x.id == itemBizEntity.id);

                    if (cvBizinRq == null)
                    {
                        await _unitOfWork.GetRepository<BizInfo>()
                                    .DeleteEntityAsync(itemBizEntity);
                    }
                }
            }
            else
            {
                foreach (var itemBizEntity in bizInfoEntityList)
                {
                    await _unitOfWork.GetRepository<BizInfo>()
                                    .DeleteEntityAsync(itemBizEntity);
                }
            }

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

        private CvInfoRequest ValidateRequest(CvInfoRequest request)
        {
            if(request.lang1_hearing < 1 || request.lang1_hearing > 3)
            {
                request.lang1_hearing = null;
            }
            if(request.lang1_speaking < 1 || request.lang1_speaking > 3)
            {
                request.lang1_speaking = null;
            }
            if(request.lang1_reading < 1 || request.lang1_reading > 3)
            {
                request.lang1_reading = null;
            }
            if(request.lang1_writing < 1 || request.lang1_writing > 3)
            {
                request.lang1_writing = null;
            }
            if(request.lang2_hearing < 1 || request.lang2_hearing > 3)
            {
                request.lang2_hearing = null;
            }
            if(request.lang2_speaking < 1 || request.lang2_speaking > 3)
            {
                request.lang2_speaking = null;
            }
            if(request.lang2_reading < 1 || request.lang2_reading > 3)
            {
                request.lang2_reading = null;
            }
            if(request.lang2_writing < 1 || request.lang2_writing > 3)
            {
                request.lang2_writing = null;
            }

            return request;
        }
    }
}
