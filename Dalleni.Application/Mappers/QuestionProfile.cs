using AutoMapper;
using Dalleni.Application.DTOs.Responses.Answers;
using Dalleni.Application.DTOs.Responses.Questions;
using Dalleni.Application.DTOs.Responses.Tags;
using Dalleni.Application.ExternalServicesAbstractions;
using Dalleni.Domin.Models;
using System.Linq;

namespace Dalleni.Application.Mappings
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {

            CreateMap<Question, QuestionDetailsResponseDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
                .ForMember(dest => dest.AuthorName,opt => opt.MapFrom(src => src.User != null ? src.User.UserName : string.Empty))
                .ForMember(dest => dest.AuthorProfileImageUrl, opt => opt.MapFrom(src => src.User != null ? src.User.ProfileImageUrl : null))
                .ForMember(dest => dest.AuthorReputation,  opt => opt.MapFrom(src => src.User != null ? src.User.Reputation : 0))
                .ForMember(dest => dest.AnswersCount, opt => opt.MapFrom(src => src.Answers != null ? src.Answers.Count : 0))
                .ForMember(dest => dest.Tags,opt => opt.MapFrom(src => src.QuestionTags != null ? src.QuestionTags.Select(qt => qt.Tag).Where(t => t != null).ToList() : new List<Tag>()))
                .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers != null ? src.Answers.OrderByDescending(a => a.CreatedAt).ToList() : new List<Answer>()))
                .ForMember(dest => dest.IsClosed, opt => opt.MapFrom(src => src.IsClosed));

            CreateMap<Question, QuestionSummaryDto>()
                .ForMember(dest => dest.AuthorName,
                    opt => opt.MapFrom(src =>
                        src.User != null ? src.User.UserName : string.Empty))

                .ForMember(dest => dest.AnswersCount,
                    opt => opt.MapFrom(src =>
                        src.Answers.Count))

                .ForMember(dest => dest.Tags,
                    opt => opt.MapFrom(src =>
                        src.QuestionTags.Select(qt => new TagDto
                        {
                            Id = qt.Tag.Id,
                            Name = qt.Tag.Name,
                            Slug = qt.Tag.Slug,
                            QuestionCount = qt.Tag.UsageCount
                        })));
            CreateMap<Question, QuestionSearchDocument>()
                .ForMember(dest => dest.Id,opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Tags,opt => opt.MapFrom(src => src.QuestionTags != null? src.QuestionTags.Select(qt => qt.Tag != null ? qt.Tag.Name : string.Empty)
                            .Where(t => !string.IsNullOrEmpty(t)).ToList()    : new List<string>()))
                .ForMember(dest => dest.CategoryName,opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
                .ForMember(dest => dest.AnswersCount,opt => opt.MapFrom(src => src.Answers != null ? src.Answers.Count : 0))
                .ForMember(dest => dest.HasAcceptedAnswer,opt => opt.MapFrom(src => src.AcceptedAnswerId.HasValue));

            // ===== QuestionSearchDocument to QuestionDetailsResponseDto =====
            CreateMap<QuestionSearchDocument, QuestionDetailsResponseDto>()
                .ForMember(dest => dest.Id,opt => opt.MapFrom(src => Guid.Parse(src.Id)))
                .ForMember(dest => dest.Title,opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content,opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.CategoryName,opt => opt.MapFrom(src => src.CategoryName))
                .ForMember(dest => dest.UpVotes,opt => opt.MapFrom(src => src.UpVotes))
                .ForMember(dest => dest.DownVotes,opt => opt.MapFrom(src => src.DownVotes))
                .ForMember(dest => dest.Views,opt => opt.MapFrom(src => src.Views))
                .ForMember(dest => dest.AnswersCount,opt => opt.MapFrom(src => src.AnswersCount))
                .ForMember(dest => dest.Score,opt => opt.MapFrom(src => src.Score))
                .ForMember(dest => dest.CreatedAt,opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.AcceptedAnswerId,opt => opt.MapFrom(src => src.HasAcceptedAnswer ? (Guid?)null : (Guid?)null))
                // These properties don't exist in QuestionSearchDocument, 
                // so we ignore them or set default values
                .ForMember(dest => dest.CategoryId,opt => opt.Ignore())
                .ForMember(dest => dest.UserId,opt => opt.Ignore())
                .ForMember(dest => dest.AuthorName,opt => opt.Ignore())
                .ForMember(dest => dest.AuthorProfileImageUrl,opt => opt.Ignore())
                .ForMember(dest => dest.AuthorReputation,opt => opt.Ignore())
                .ForMember(dest => dest.IsClosed,opt => opt.Ignore())
                .ForMember(dest => dest.AcceptedAnswerId,opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt,opt => opt.Ignore())
                .ForMember(dest => dest.Tags,opt => opt.MapFrom(src => src.Tags != null ? src.Tags.Select(tagName => new TagDto { Name = tagName }).ToList()    : new List<TagDto>()))
                .ForMember(dest => dest.Answers,opt => opt.Ignore());

        }
    }
}