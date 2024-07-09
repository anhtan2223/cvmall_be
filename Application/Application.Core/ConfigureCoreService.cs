using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Application.Common.Abstractions;
using Application.Core.Services.Core;
using Application.Core.Interfaces.Core;

namespace Application.Core.Extensions
{
    public static class ConfigureCoreService
    {
        public static IServiceCollection AddCoreService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ILocalizeServices, LocalizeServices>();
            services.AddScoped<IAuthServices, AuthServices>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IRoleServices, RoleServices>();
            services.AddScoped<IFunctionServices, FunctionServices>();
            services.AddScoped<IMasterCodeServices, MasterCodeServices>();
            services.AddScoped<ICvInfoServices, CvInfoServices>();
            services.AddScoped<ITechnicalCategoryServices, TechnicalCategoryServices>();
            services.AddScoped<ITechnicalServices, TechnicalServices>();
            services.AddScoped<IBizInfoServices, BizInfoServices>();
            services.AddScoped<IEmployeeServices, EmployeeServices>();
            services.AddScoped<IPositionServices, PositionServices>();
            services.AddScoped<ICvTechnicalInfoServices, CvTechnicalInfoServices>();
            services.AddScoped<ITimesheetServices, TimesheetServices>();

            services.AddScoped<IResourceServices, ResourceServices>();
            services.AddScoped<ILogServices, LogServices>();
            services.AddScoped<IDepartmentServices, DepartmentServices>();

            return services;
        }
    }
}
