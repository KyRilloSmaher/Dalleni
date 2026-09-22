using AutoMapper;
using Dalleni.Application.DTOs.Responses.Votes;
using Dalleni.Domin.Models;

namespace Dalleni.Application.Mappers
{
    public class VoteProfile : Profile
    {
        public VoteProfile()
        {
            CreateMap<Vote, VotedAnswerResponse>()
                .ForMember(dest => dest.VoteId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Answer, opt => opt.MapFrom(src => src.Answer))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Answer.CreatedAt));
            CreateMap<Vote, VotedQuestionResponse>()
                .ForMember(dest => dest.VoteId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.Question.CreatedAt));
        }
    }
}
