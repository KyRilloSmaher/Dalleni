using Dalleni.Application.DTOs.Requests.Users;
using Dalleni.Application.DTOs.Responses.Users;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Features.Users.Commands.UpdateRrofileImage
{
    public record UpdateProfileImageCommand
    (
     UpdateUserProfileImage request
    ):IRequest<Response<string>>;
}
