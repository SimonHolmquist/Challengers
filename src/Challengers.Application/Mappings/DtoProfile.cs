using AutoMapper;
using Challengers.Application.DTOs;
using Challengers.Domain.Entities;

namespace Challengers.Application.Mappings;

public class DtoProfile : Profile
{
    public DtoProfile()
    {
        CreateMap<PlayerDto, MalePlayer>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Skill, opt => opt.MapFrom(src => src.Skill))
            .ForMember(dest => dest.Strength, opt => opt.MapFrom(src => src.Strength ?? 0))
            .ForMember(dest => dest.Speed, opt => opt.MapFrom(src => src.Speed ?? 0));

        CreateMap<PlayerDto, FemalePlayer>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Skill, opt => opt.MapFrom(src => src.Skill))
            .ForMember(dest => dest.ReactionTime, opt => opt.MapFrom(src => src.ReactionTime ?? 0));
    }
}
