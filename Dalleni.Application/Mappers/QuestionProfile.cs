using AutoMapper;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Application.ExternalServicesAbstractions;
using Dalleni.Domin.Models;

namespace Dalleni.Application.Mappers
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<Question, QuestionDetailsResponseDto>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : "Unknown"))
                .ForMember(dest => dest.AuthorProfileImageUrl, opt => opt.MapFrom(src => src.User != null ? src.User.ProfileImageUrl : null))
                .ForMember(dest => dest.AuthorReputation, opt => opt.MapFrom(src => src.User != null ? src.User.Reputation : 0))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "Uncategorized"))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.QuestionTags.Select(qt => qt.Tag)));

            CreateMap<Question, QuestionSummaryDto>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.User != null ? src.User.FullName : "Unknown"))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.QuestionTags.Select(qt => qt.Tag)));

            CreateMap<Question, QuestionSearchDocument>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "Uncategorized"))
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.QuestionTags.Select(qt => qt.Tag.Name)))
                .ForMember(dest => dest.HasAcceptedAnswer, opt => opt.MapFrom(src => src.AcceptedAnswerId.HasValue));
        }
    }
}
