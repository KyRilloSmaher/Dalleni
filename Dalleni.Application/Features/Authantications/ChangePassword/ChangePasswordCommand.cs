using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Dalleni.Domin.ResponsePattern;
using Dalleni.Application.DTOs.Requests.Auth;
namespace Dalleni.Application.Features.Authantications.ChangePassword
{
    public record ChangePasswordCommand(string UserId, ChangePasswordRequestDto dto) : IRequest<Response<bool>>;
}
