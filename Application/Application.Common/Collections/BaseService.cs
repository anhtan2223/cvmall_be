using AutoMapper;

namespace Application.Common.Abstractions
{
    public class BaseService : IBaseService
    {
        protected IMapper _mapper { get; set; }
        protected IUnitOfWork _unitOfWork { get; set; }

        public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
