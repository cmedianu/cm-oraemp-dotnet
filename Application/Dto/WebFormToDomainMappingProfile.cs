using AutoMapper;
using OraEmp.Domain.Entities;

namespace OraEmp.Application.Dto
{
    public class WebFormToDomainMappingProfile : Profile
    {
        public WebFormToDomainMappingProfile()
        {
            CreateMap<Department, DepartmentForm>().ReverseMap();
            /*
            CreateMap<Department, DepartmentForm>()
                .ForMember(dst => dst.Obj,
                    opt => opt.MapFrom(src => src)).ReverseMap();
                    */
        }
    }
}