using AutoMapper;
using Dalleni.Application.DTOs.Requests.Ratings;
using Dalleni.Application.DTOs.Responses.Ratings;
using Dalleni.Application.DTOs.Responses.Tags;
using Dalleni.Application.Features.Ratings.Commands.CreateRating;
using Dalleni.Domin.Models;

namespace Dalleni.Application.Mappers
{
    public class RatingProfile : Profile
    {
        public RatingProfile()
        {
            CreateMap<Rating, RatingDto>();
            CreateMap<CreateRatingCommand, Rating>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.ServiceId, opt => opt.MapFrom(src => src.Dto.ServiceId))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Dto.Value))
            .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Dto.Comment))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Dto.UserName));
            CreateMap<RatingDto, Rating>();
            CreateMap<UpdateRatingRequestDto, Rating>();
                
        }
    }
}
