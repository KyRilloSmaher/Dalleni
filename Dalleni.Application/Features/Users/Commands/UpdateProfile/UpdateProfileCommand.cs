using Dalleni.Application.DTOs.Requests.Users;
using Dalleni.Application.DTOs.Responses.Users;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Features.Users.Commands.UpdateProfile
{
    public record UpdateProfileCommand
    (
        UpdateUserAccount request
    ) :IRequest<Response<UserResponseDto>>;
}
