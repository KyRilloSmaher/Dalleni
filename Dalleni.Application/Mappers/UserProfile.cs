using AutoMapper;
using Dalleni.Application.DTOs.Requests.Users;
using Dalleni.Application.DTOs.Responses.Users;
using Dalleni.Domin.Models;

namespace Dalleni.Application.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserResponseDto>();

            CreateMap<UpdateUserAccount, ApplicationUser>()
                   .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FirstName +" "+src.LastName));
        }
    }
}
