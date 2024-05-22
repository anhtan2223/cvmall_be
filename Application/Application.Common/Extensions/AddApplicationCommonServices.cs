using Application.Common.Abstractions;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common.Extensions
{
    public static class AddApplicationCommonServices
    {
        public static IServiceCollection AddCoreExtention(this IServiceCollection services)
        {
            // FluentValidation
            services.AddFluentValidation(c =>
            {
                c.DisableDataAnnotationsValidation = true;
                c.ImplicitlyValidateChildProperties = true;
                //c.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            });


            services.AddScoped<IBaseService, BaseService>();

            return services;
        }
    }
}
