using Dalleni.Application.DTOs.Responses.Auth;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Dalleni.Application.Features.Authantications.GooleSignin
{
    public record GooleSigninCommand(string email,string Surname,string Username,string picture ,string nameIdentifier) : IRequest<Response<TokenReponseDto>>;
}
