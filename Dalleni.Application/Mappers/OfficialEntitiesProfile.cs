using AutoMapper;
using Dalleni.Application.DTOs.Requests.OfficialEntities;
using Dalleni.Application.DTOs.Responses.OfficialEntities;
using Dalleni.Domin.Models;

namespace Dalleni.Application.Mappers
{
    public class OfficialEntitiesProfile : Profile
    {
        public OfficialEntitiesProfile()
        {
            CreateMap<OfficialEntity, OfficialEntityDto>();

            CreateMap<CreateOfficialEntityRequestDto, OfficialEntity>()
                    .ForMember(dest => dest.LogoUrl, opt => opt.Ignore())
                    .ForMember(dest => dest.Id, opt => opt.Ignore())
                    .ForMember(dest => dest.IsVerified, opt => opt.Ignore())
                    .ForMember(dest => dest.Services, opt => opt.Ignore())
                    .ForMember(dest => dest.Members, opt => opt.Ignore());

            CreateMap<UpdateOfficialEntityRequestDto, OfficialEntity>()
                    .ForMember(dest => dest.Services, opt => opt.Ignore())
                    .ForMember(dest => dest.Members, opt => opt.Ignore());
        }
    }
}
