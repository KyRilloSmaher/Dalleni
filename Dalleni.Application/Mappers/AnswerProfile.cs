using AutoMapper;
using Dalleni.Application.DTOs.Responses.Answers;
using Dalleni.Domin.Models;

namespace Dalleni.Application.Mappers
{
    public class AnswerProfile : Profile
    {
        public AnswerProfile()
        {
            CreateMap<Answer, AnswerDto>()
                        .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.User.FullName ?? "Unknown"))
                        .ForMember(dest => dest.AuthorProfileImageUrl, opt => opt.MapFrom(src => src.User.ProfileImageUrl))
                        .ForMember(dest => dest.AuthorReputation, opt => opt.MapFrom(src => src.User != null ? src.User.Reputation : 0));
        }
    }
}
