using AutoMapper;
using Backend.DTOs;
using Backend.Models;

namespace Backend.AutoMappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            CreateMap<BeerInsertDTO, Beer>()
                .ForMember(dest => dest.BeerName,opt => opt.MapFrom(src => src.Name));
            CreateMap<Beer, BeerDTO>()
                .ForMember(dto => dto.Id,
                           m => m.MapFrom(b => b.BeerID))
                .ForMember(dto => dto.Name,
                           m => m.MapFrom(b => b.BeerName));
            CreateMap<BeerUpdateDTO, Beer>()
                .ForMember(dest => dest.BeerName, opt => opt.MapFrom(src => src.Name));
        }

    }
}
