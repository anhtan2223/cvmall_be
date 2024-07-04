using AutoMapper;
using Framework.Core.Abstractions;
using Framework.Core.Collections;
using Application.Core.Contracts;
using Domain.Entities;
using Framework.Core.Extensions;

namespace WebAPI.ProfileMapper
{
    /// <summary>
    /// 
    /// </summary>
    public class CoreProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public CoreProfile()
        {
            CreateMap<UserRequest, User>();
            CreateMap<PagedList<User>, PagedList<UserResponse>>();
            CreateMap<IPagedList<User>, PagedList<UserResponse>>();
            CreateMap<User, UserResponse>()
                .ForMember(dest => dest.birthday, opt => opt.MapFrom(src => src.birthday.ToDateString()))
                .ForMember(dest => dest.role_cds, opt => opt.MapFrom(src => src.user_roles.Select(x=> x.role_cd).ToList()))
                .ForMember(dest => dest.role_names, opt => opt.MapFrom(src => String.Join(", ", src.user_roles.Select(x => x.role_cd).ToList())));

            CreateMap<RoleRequest, Role>()
                .ForMember(dest => dest.permissions, opt => opt.Ignore());
            CreateMap<IPagedList<Role>, PagedList<RoleResponse>>();
            CreateMap<PagedList<Role>, PagedList<RoleResponse>>();
            CreateMap<Role, RoleResponse>()
                .ForMember(dest => dest.permissions, opt => opt.MapFrom(src => src.permissions.Select(x => x.function.code).ToList()));

            CreateMap<FunctionRequest, Function>();
            CreateMap<IPagedList<Function>, PagedList<FunctionResponse>>();
            CreateMap<PagedList<Function>, PagedList<FunctionResponse>>();
            CreateMap<Function, FunctionResponse>();
            //.ForMember(dest => dest.parent_cd, opt => opt.MapFrom(src => src.parent.code));

            CreateMap<ResourceRequest, Resource>();
            CreateMap<IPagedList<Resource>, PagedList<ResourceResponse>>();
            CreateMap<PagedList<Resource>, PagedList<ResourceResponse>>();
            CreateMap<Resource, ResourceResponse>();

            CreateMap<MasterCodeRequest, MasterCode>();
            CreateMap<IPagedList<MasterCode>, PagedList<MasterCodeRespose>>();
            CreateMap<PagedList<MasterCode>, PagedList<MasterCodeRespose>>();
            CreateMap<MasterCode, MasterCodeRespose>();

            CreateMap<CvInfoRequest, CvInfo>();
            CreateMap<IPagedList<CvInfo>, PagedList<CvInfoResponse>>();
            CreateMap<PagedList<CvInfo>, PagedList<CvInfoResponse>>();
            CreateMap<CvInfo, CvInfoResponse>();

            CreateMap<CvTechnicalInfoRequest, CvTechnicalInfo>();
            CreateMap<IPagedList<CvTechnicalInfo>, PagedList<CvTechInfoResponse>>();
            CreateMap<PagedList<CvTechnicalInfo>, PagedList<CvTechInfoResponse>>();
            CreateMap<CvTechnicalInfo, CvTechInfoResponse>();

            CreateMap<BizInfoRequest, BizInfo>();
            CreateMap<IPagedList<BizInfo>, PagedList<BizInfoResponse>>();
            CreateMap<PagedList<BizInfo>, PagedList<BizInfoResponse>>();
            CreateMap<BizInfo, BizInfoResponse>();

            CreateMap<TechnicalCategoryRequest, TechnicalCategory>();
            CreateMap<IPagedList<TechnicalCategory>, PagedList<TechnicalCategoryResponse>>();
            CreateMap<PagedList<TechnicalCategory>, PagedList<TechnicalCategoryResponse>>();
            CreateMap<TechnicalCategory, TechnicalCategoryResponse>();

            CreateMap<TechnicalRequest, Technical>();
            CreateMap<IPagedList<Technical>, PagedList<TechnicalResponse>>();
            CreateMap<PagedList<Technical>, PagedList<TechnicalResponse>>();
            CreateMap<Technical, TechnicalResponse>();

            CreateMap<TimesheetRequest, Timesheet>();
            CreateMap<IPagedList<Timesheet>, PagedList<TimesheetResponse>>();
            CreateMap<PagedList<Timesheet>, PagedList<TimesheetResponse>>();
            CreateMap<Timesheet, TimesheetResponse>();
        }
    }
}
