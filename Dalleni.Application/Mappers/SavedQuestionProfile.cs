 using AutoMapper;
using Dalleni.Application.DTOs.Responses.SavedQuestions;
using Dalleni.Application.ExternalServicesAbstractions;
using Dalleni.Domin.Models;

namespace Dalleni.Application.Mappers
{
 public class SavedQuestionProfile : Profile
    {
        public SavedQuestionProfile()
        {
            CreateMap<SavedQuestion, SavedQuestionDto>()
                .ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question));
        }
    }
}
