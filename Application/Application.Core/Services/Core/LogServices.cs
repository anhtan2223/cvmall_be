using Application.Common.Abstractions;
using Application.Common.Extensions;
using Application.Core.Interfaces.Core;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace Application.Core.Services.Core
{
    public class LogServices : BaseService, ILogServices
    {
        private readonly IRepository<LogAction> logActionRepository;
        private readonly IRepository<LogException> logExceptionRepository;
        private readonly ICurrentUserService serviceContext;
        private readonly IUserServices userServices;
        private readonly ILocalizeServices ls;
        private readonly IConfiguration configuration;

        public LogServices(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService serviceContext, IUserServices userServices, ILocalizeServices ls, IConfiguration configuration) : base(unitOfWork, mapper)
        {
            logActionRepository = _unitOfWork.GetRepository<LogAction>();
            logExceptionRepository = _unitOfWork.GetRepository<LogException>();
            this.serviceContext = serviceContext;
            this.userServices = userServices;
            this.ls = ls;
            this.configuration = configuration;
        }

        public Task WriteLogAction(string path, string method, string ip, string body = null)
        {
            throw new NotImplementedException();
        }

        //public async Task WriteLogAction(string path, string method, string ip, string body = "")
        //{
        //    string message = RenderMessageLog(method, path);

        //    await logActionRepository.AddEntityAsync(new LogAction
        //    {
        //        path = path,
        //        method = method,
        //        body = body,
        //        message = message,
        //        ip = ip
        //    });

        //    await _unitOfWork.SaveChangesAsync();
        //}

        public async Task WriteLogException(string function, string message, string stackTrace)
        {
            await logExceptionRepository.AddEntityAsync(new LogException
            {
                function = function,
                message = message,
                stack_trace = stackTrace,
                created_at = DateTime.Now
            });

            await _unitOfWork.SaveChangesAsync();
        }

        private string RenderMessageLog(string method, string path)
        {
            var userName = userServices.GetUserNameById((Guid)serviceContext.user_id);
            var message = ls.Get(Modules.Log, "Common", "MessageLog");
            string newMess = string.Format(message, userName, method, path);

            return newMess;
        }
    }
}
