using AutoMapper;
using Framework.Core.Collections;
using Framework.Core.Extensions;
using Application.Core.Contracts;
using Domain.Entities;
using Application.Common.Abstractions;
using Application.Core.Interfaces.Core;
using System.Data.Entity;

namespace Application.Core.Services.Core
{
    public class PositionServices : BaseService, IPositionServices
    {
        private readonly IRepository<Position> positionRepository;

        public PositionServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper)
        {
            positionRepository = _unitOfWork.GetRepository<Position>();
        }

        public async Task<PagedList<PositionResponse>> GetPaged(RequestPaged request)
        {
            var data = await positionRepository
                    .GetQuery()
                    .ExcludeSoftDeleted()
                    .SortBy(request.sort ?? "updated_at.desc")
                    .ToPagedListAsync(request.page, request.size);

            var dataMapping = _mapper.Map<PagedList<PositionResponse>>(data);

            return dataMapping;
        }

        public async Task<IList<PositionResponse>> GetList()
        {
            var data = await positionRepository
                .GetQuery()
                .ExcludeSoftDeleted()
                .SortBy("updated_at.desc").ToPagedListAsync(1, 9999);

            List<PositionResponse> dataMapping = new();

            if (data?.data?.Count > 0)
            {
                dataMapping = _mapper.Map<IList<Position>, List<PositionResponse>>(data.data);
            }

            return dataMapping;
        }

        public async Task<PositionResponse> GetById(Guid id)
        {
            var entity = positionRepository
                                  .GetQuery()
                                  .ExcludeSoftDeleted()
                                  .FilterById(id)
                                  .FirstOrDefault();

            var data = _mapper.Map<PositionResponse>(entity);

            return data;
        }

        public async Task<int> Create(PositionRequest request)
        {
            var count = 0;
            
            var isValid = positionRepository.GetQuery().Where(x => x.name == request.name).Any();
            if(isValid){
                return count ;
            }
            

            var Position = _mapper.Map<Position>(request);

            await positionRepository.AddEntityAsync(Position);

            count += await _unitOfWork.SaveChangesAsync();

            return count;
        }

        public async Task<int> Update(Guid id, PositionRequest request)
        {
            var count = 0;

            var entity = _unitOfWork
                            .GetRepository<Position>()
                            .GetQuery()
                            .FindActiveById(id)
                            .FirstOrDefault();

            if (entity == null)
                return count;

            _mapper.Map(request, entity);
            await positionRepository.UpdateEntityAsync(entity);

            count = await _unitOfWork.SaveChangesAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var entity = positionRepository.GetQuery().FindActiveById(id).FirstOrDefault();

            if (entity == null)
                return 0;

            await positionRepository.DeleteEntityAsync(entity);
            var count = await _unitOfWork.SaveChangesAsync();
            return count;
        }
    }
}
