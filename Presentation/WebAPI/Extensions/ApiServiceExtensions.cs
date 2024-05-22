using FluentValidation.AspNetCore;
using WebAPI.ProfileMapper;
using Application.Core.Services;
using Application.Common.Abstractions;
using Infrastructure.Contracts;

namespace WebAPI.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class APIServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddServiceContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AuthSetting>(configuration.GetSection("AuthSetting"));
            services.AddScoped<ICurrentUserService, ApiServiceContext>();
            
            // Adding the AutoMapper to DI container
            services.AddAutoMapper(typeof(CoreProfile));

            // FluentValidation intercept
            services.AddTransient<IValidatorInterceptor, ValidatorInterceptor>();

            return services;
        }


    }
}
