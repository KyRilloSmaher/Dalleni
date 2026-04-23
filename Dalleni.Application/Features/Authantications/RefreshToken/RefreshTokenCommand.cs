using System;
using System.Collections.Generic;
using System.Text;
using MediatR;
using Dalleni.Domin.ResponsePattern;
using Dalleni.Application.DTOs.Requests.Auth;
using Dalleni.Application.DTOs.Responses.Auth;
namespace Dalleni.Application.Features.Authantications.RefreshToken
{
    public record RefreshTokenAsyncCommand(TokenRequestDto dto) : IRequest<Response<TokenReponseDto>>;
}
