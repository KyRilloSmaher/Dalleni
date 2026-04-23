using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Dalleni.Domin.ResponsePattern;
using Dalleni.Application.DTOs.Requests.Auth;
using Dalleni.Application.DTOs.Responses.Auth;
namespace Dalleni.Application.Features.Authantications.Login
{
    public record LoginCommand(LoginRequestDto dto) : IRequest<Response<TokenReponseDto>>;
}
