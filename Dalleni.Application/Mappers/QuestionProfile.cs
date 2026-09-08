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
                .ForMember(dest => dest.id,opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.tags,opt => opt.MapFrom(src => src.QuestionTags != null? src.QuestionTags.Select(qt => qt.Tag != null ? qt.Tag.Name : string.Empty)
                            .Where(t => !string.IsNullOrEmpty(t)).ToList()    : new List<string>()))
                .ForMember(dest => dest.categoryName,opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : string.Empty))
                .ForMember(dest => dest.answersCount,opt => opt.MapFrom(src => src.Answers != null ? src.Answers.Count : 0))
                .ForMember(dest => dest.hasAcceptedAnswer,opt => opt.MapFrom(src => src.AcceptedAnswerId.HasValue));

            // ===== QuestionSearchDocument to QuestionSummaryDto =====
            CreateMap<QuestionSearchDocument, QuestionSummaryDto>()
                .ForMember(dest => dest.Id,opt => opt.MapFrom(src => Guid.Parse(src.id)))
                .ForMember(dest => dest.Title,opt => opt.MapFrom(src => src.title))
                .ForMember(dest => dest.Content,opt => opt.MapFrom(src => src.content))
                .ForMember(dest => dest.CategoryName,opt => opt.MapFrom(src => src.categoryName))
                .ForMember(dest => dest.UpVotes,opt => opt.MapFrom(src => src.upVotes))
                .ForMember(dest => dest.DownVotes,opt => opt.MapFrom(src => src.downVotes))
                .ForMember(dest => dest.Views,opt => opt.MapFrom(src => src.views))
                .ForMember(dest => dest.AnswersCount,opt => opt.MapFrom(src => src.answersCount))
                .ForMember(dest => dest.Score,opt => opt.MapFrom(src => src.score))
                .ForMember(dest => dest.CreatedAt,opt => opt.MapFrom(src => src.createdAt))
                .ForMember(dest => dest.UserId,opt => opt.Ignore())
                .ForMember(dest => dest.AuthorName,opt => opt.Ignore())
                .ForMember(dest => dest.AuthorProfileImageUrl,opt => opt.Ignore())
                .ForMember(dest => dest.IsClosed,opt => opt.Ignore())
                .ForMember(dest => dest.Tags,opt => opt.MapFrom(src => src.tags != null ? src.tags.Select(tagName => new TagDto { Name = tagName }).ToList(): new List<TagDto>()));


        }
    }
}