using Application.Common.Abstractions;
using Application.Core.Contracts;
using Application.Core.Interfaces.Core;
using AutoMapper;
using Domain.Entities;
using Framework.Core.Collections;
using Framework.Core.Extensions;

namespace Application.Core.Services.Core
{

    public class ResourceServices : BaseService, IResourceServices
    {
        private readonly IRepository<Resource> resourceRepository;

        public ResourceServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper)
        {
            resourceRepository = _unitOfWork.GetRepository<Resource>();
        }

        public async Task<PagedList<ResourceResponse>> GetPaged(ResourcePagedRequest request)
        {
            var data = resourceRepository
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .Where(x => (string.IsNullOrEmpty(request.screen) ? true : x.screen.Equals(request.screen))
                                            && (string.IsNullOrEmpty(request.module) ? true : x.module.Equals(request.module)))
                                    .SortBy(request.sort)
                                    .ToPagedList(request.page, request.size);

            var dataMapping = _mapper.Map<PagedList<ResourceResponse>>(data);
            return dataMapping;
        }

        public List<ResourceResponse> GetByScreen(string lang, string module, string screen)
        {
            var data = resourceRepository
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .Where(x => x.lang == lang)
                                    .Where(x => x.module == module)
                                    .Where(x => x.screen == screen)
                                    .ToList();

            var dataMapping = _mapper.Map<List<ResourceResponse>>(data);
            return dataMapping;
        }

        public async Task<ResourceResponse> GetById(Guid id)
        {
            var entity = resourceRepository
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .FirstOrDefault();

            var data = _mapper.Map<ResourceResponse>(entity);
            return data;
        }

        public async Task<int> Create(ResourceRequest request)
        {
            var entity = _mapper.Map<Resource>(request);
            await resourceRepository.AddEntityAsync(entity);
            var count = await _unitOfWork.SaveChangesAsync();

            return count;
        }

        public async Task<int> Update(Guid id, ResourceRequest request)
        {
            var count = 0;
            var entity = _unitOfWork
                            .GetRepository<Resource>()
                            .GetQuery()
                            .FindActiveById(id)
                            .FirstOrDefault();

            if (entity == null)
                return count;

            _mapper.Map(request, entity);
            await resourceRepository.UpdateEntityAsync(entity);
            count = await _unitOfWork.SaveChangesAsync();
            return count;

        }

        public async Task<int> Delete(Guid id)
        {
            var entity = resourceRepository.GetQuery().FindActiveById(id).FirstOrDefault();

            if (entity == null)
                return 0;

            await resourceRepository.DeleteEntityAsync(entity);
            var count = await _unitOfWork.SaveChangesAsync();
            return count;
        }

        public async Task<PagedList<string>?> GetListScreen(PagedRequest request)
        {
            return resourceRepository.GetQuery()
                        .ExcludeSoftDeleted()
                        .Select(e => e.screen)
                        .Distinct()
                        .SortBy(request.sort)
                        .ToPagedList(request.page, request.size);
        }

        public async Task<PagedList<string>?> GetScreensByModule(ResourcePagedRequest request)
        {
            return resourceRepository.GetQuery()
                        .ExcludeSoftDeleted()
                        .Where(x => x.module.Equals(request.module))
                        .Select(e => e.screen)
                        .Distinct()
                        .SortBy(request.sort)
                        .ToAllPageList();
        }
    }
}
