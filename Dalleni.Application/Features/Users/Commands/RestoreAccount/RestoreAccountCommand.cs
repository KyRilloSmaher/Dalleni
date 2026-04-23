using Dalleni.Application.DTOs.Requests.Users;
using Dalleni.Application.DTOs.Responses.Auth;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dalleni.Application.Features.Users.Commands.RestoreAccount
{
    public record RestoreAccountCommand
    (
        RestoreAccountRequest request
    ) : IRequest<Response<TokenReponseDto>>;
}
