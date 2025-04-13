using AutoMapper;
using MiddlewareService.Domain.Dtos;
using MiddlewareService.Domain.Entities;

namespace Application.Mappings
{
    public class ProvinceProfile : Profile
    {
        public ProvinceProfile()
        {
            CreateMap<ProvinceDto, Province>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
        }
    }
}
