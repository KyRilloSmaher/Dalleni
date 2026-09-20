using AutoMapper;
using Dalleni.Application.DTOs.Responses.Notifications;
using Dalleni.Domin.Models;

namespace Dalleni.Application.Mapping.Profiles
{
    /// <summary>
    /// Defines mappings for notification entities and DTOs.
    /// </summary>
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationResponseDto>();
        }
    }
}