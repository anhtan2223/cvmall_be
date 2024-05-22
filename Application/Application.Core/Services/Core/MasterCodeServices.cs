using AutoMapper;
using Application.Core.Contracts;
using Domain.Entities;
using Framework.Core.Collections;
using Framework.Core.Extensions;
using Application.Common.Abstractions;
using Application.Core.Interfaces.Core;

namespace Application.Core.Services.Core
{
    public class MasterCodeServices : BaseService, IMasterCodeServices
    {
        private readonly IRepository<MasterCode> masterCodeRepository;
        public MasterCodeServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper)
        {
            masterCodeRepository = _unitOfWork.GetRepository<MasterCode>();
        }

        public async Task<PagedList<MasterCodeRespose>> GetPaged(PagedRequest request)
        {

            var data = masterCodeRepository
                        .GetQuery()
                        .Where(x => !string.IsNullOrEmpty(request.search) ? x.type.Contains(request.search) || x.value.Contains(request.search) : true)
                        .SortBy(request.sort)
                        .ExcludeSoftDeleted()
                        .ToPagedList(request.page, request.size);

            var dataMapping = _mapper.Map<PagedList<MasterCodeRespose>>(data);
            return dataMapping;
        }

        public async Task<PagedList<MasterCodeRespose>> GetByTypePaged(MasterCodePagedRequest request)
        {

            var data = masterCodeRepository
                        .GetQuery()
                        .Where(x => !string.IsNullOrEmpty(request.type) ? x.type.Equals(request.type) : false)
                        .ExcludeSoftDeleted()
                        .SortBy(request.sort)
                        .ToPagedList(request.page, request.size);

            var dataMapping = _mapper.Map<PagedList<MasterCodeRespose>>(data);
            return dataMapping;
        }

        public async Task<MasterCodeRespose> GetById(Guid id)
        {
            var masterCode = masterCodeRepository
                                  .GetQuery()
                                  .ExcludeSoftDeleted()
                                  .FilterById(id)
                                  .FirstOrDefault();

            var data = _mapper.Map<MasterCodeRespose>(masterCode);
            return data;
        }

        public async Task<int> Create(MasterCodeRequest request)
        {
            var masterCode = _mapper.Map<MasterCode>(request);
            await masterCodeRepository.AddEntityAsync(masterCode);
            var count = await _unitOfWork.SaveChangesAsync();
            return count;
        }

        public async Task<int> Update(Guid id, MasterCodeRequest request)
        {
            var count = 0;
            var masterCode = masterCodeRepository
                            .GetQuery()
                            .FindActiveById(id)
                            .FirstOrDefault();

            if (masterCode == null)
                return count;

            _mapper.Map(request, masterCode);
            await masterCodeRepository.UpdateEntityAsync(masterCode);
            count = await _unitOfWork.SaveChangesAsync();
            return count;

        }

        public async Task<int> Delete(Guid id)
        {
            var masterCode = masterCodeRepository.GetQuery().FindActiveById(id).FirstOrDefault();

            if (masterCode == null)
                return 0;

            await masterCodeRepository.DeleteEntityAsync(masterCode);
            var count = await _unitOfWork.SaveChangesAsync();
            return count;
        }

        public async Task<int> Delete(Guid[] ids)
        {
            var masterCode = masterCodeRepository.GetQuery().FilterByIds(ids).ToList();

            foreach (var item in masterCode)
            {
                await masterCodeRepository.DeleteEntityAsync(item);
            }
            var count = await _unitOfWork.SaveChangesAsync();
            return count;
        }


    }
}
