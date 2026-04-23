using AutoMapper;
using Dalleni.Application.DTOs.Responses.Tags;
using Dalleni.Domin.Models;

namespace Dalleni.Application.Mappers
{
    public class TagProfile : Profile
    {
        public TagProfile()
        {
            CreateMap<Tag, TagDto>()
                .ForMember(dest => dest.QuestionCount, opt => opt.MapFrom(src => src.UsageCount));
        }
    }
}
