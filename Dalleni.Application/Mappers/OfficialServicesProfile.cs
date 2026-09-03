using AutoMapper;
using Dalleni.Application.DTOs.Requests.Services;
using Dalleni.Application.DTOs.Responses.Services;
using Dalleni.Domin.Models;

namespace Dalleni.Application.Mappers
{
    public class OfficialEServicesProfile : Profile
    {
        public OfficialEServicesProfile()
        {
            CreateMap<Service, ServiceDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.OfficialEntityName, opt => opt.MapFrom(src => src.OfficialEntity.Name))
                .ForMember(dest => dest.OfficialEntityId, opt => opt.MapFrom(src => src.OfficialEntity.Id))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Category.Id))
                .ForMember(dest => dest.IsOfficialEntityVerified, opt => opt.MapFrom(src => src.OfficialEntity.IsVerified));

            CreateMap<CreateServiceRequestDto, Service>();
            CreateMap<UpdateServiceRequestDto, Service>();
        }
    }
}
