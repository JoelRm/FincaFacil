using AutoMapper;
using Domain.Dtos;
using MiddlewareService.Domain.Entities;

namespace Application.Mappings
{
    class BankProfile : Profile
    {
        public BankProfile()
        {
            CreateMap<BankDto, Bank>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Disabled, opt => opt.MapFrom(src => src.Disabled));
        }
    }
}
