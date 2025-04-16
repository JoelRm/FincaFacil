using AutoMapper;
using Domain.Dtos;
using MiddlewareService.Domain.Entities;

namespace Application.Mappings
{
    class BankMovementProfile : Profile
    {
        public BankMovementProfile()
        {
            CreateMap<BankMovementDto, BankMovement>()
                .ForMember(dest => dest.MovementId, opt => opt.MapFrom(src => src.MovementId))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Concept, opt => opt.MapFrom(src => src.Concept))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Total));
        }
    }
}
